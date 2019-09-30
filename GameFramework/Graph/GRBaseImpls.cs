using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
namespace GameFramework
{

	abstract public class GRBaseImpls : GameEventDispatcher, IObjectPlugin, IProcess
	{
		private string _controlId;
		protected GRClient g_mgr;
		protected LGGRBaseImpls m_ctrl;
		protected object m_gr;
		//protected object m_gr_shadow;
		 
		protected Dictionary<string,List<IGREffectParticles>> m_effectLoad = new Dictionary<string,List<IGREffectParticles>>();//保存添加的特效

		public GRBaseImpls( clientBase m )
		{
			g_mgr = m as GRClient;		 
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
		abstract public void init();
		 
		
		abstract public void dispose();
		 

		abstract protected void onSetGraphImpl();
		 
		abstract protected void onSetSceneCtrl();
	 
		public void setGraphImpl( object gr )
		{
			m_gr = gr;
			onSetGraphImpl();
		}
		public void setSceneCtrl( LGGRBaseImpls ctrl )
		{ 
			m_ctrl = ctrl;
			onSetSceneCtrl();
		}

        virtual public void initLg(lgGDBase lgbase)
        {

        }


		protected IGREffectParticles addEffect( string effid, float x, float y, float z, bool single=true )
		{
            IGREffectParticles eff = createEffect(effid, single);

            if(eff != null)
			{
				(eff as IGREntity).x = x;
            	(eff as IGREntity).y = y;
            	(eff as IGREntity).z = z;
			}
           
            return eff;
		}

        protected IGREffectParticles createEffect(string effid, bool single = true)
        {
            IGREffectParticles eff = null;
            if (!m_effectLoad.ContainsKey(effid))
            {//todo need smart way
                m_effectLoad[effid] = new List<IGREffectParticles>();

            }
            List<IGREffectParticles> arr = m_effectLoad[effid];

            if (single && arr.Count > 0)
            {
                eff = arr[0];
                eff.stop();
            }
            else
            {
                eff = this.g_mgr.createEffect(effid);

                if (eff != null) arr.Add(eff);
            }

            return eff;
        }  
		
		protected IGREffectParticles attachEffect( string effid, string attachID, bool single=true )
		{
            IGREffectParticles eff = createEffect(effid, single);
			        
            if(eff != null )
			{
				IGRCharacter grc = ( m_gr as IGRCharacter );
				if( grc!= null ) 
				{
					grc.attachEntity( attachID, eff );			
				}
				else
				{
					DebugTrace.print( "attachEffect should be IGRCharacter!" );
				}				
			}
           
            return eff;
		}  

		protected void deleteEffects()
		{
			foreach( List<IGREffectParticles> val in m_effectLoad.Values )
			{
				
				foreach( IGREntity eff in val )
				{
					eff.dispose();
					this.g_mgr.deleteEntity( eff );
				}
			
			}
		}

        protected IGREffectParticles attachEffect(string effid, bool single = true)
        {
            IGREffectParticles eff = createEffect(effid, single);

            if (eff != null)
            {
                GREntity3D grc = (m_gr as GREntity3D);
                if (grc != null)
                {
                    //if (grc.rootObj != null && !grc.rootObj.contains(eff.graphObject as IGraphObject3D))
                    //{
                    //    grc.rootObj.addChild(eff.graphObject as IGraphObject3D);
                    //}
                }
                else
                {
                    DebugTrace.print("attachEffect should be IGRCharacter!");
                }
            }

            return eff;
        }

        protected IGREffectParticles getSingleEffect(string effid)
        {
            IGREffectParticles eff = null;
            if (m_effectLoad.ContainsKey(effid))
            {
                List<IGREffectParticles> arr = m_effectLoad[effid];
                if (arr.Count > 0)
                {
                    eff = arr[0];
                }
            }
            return eff;
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