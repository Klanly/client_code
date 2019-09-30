using System;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
    class AttAniManager
    {
        private List<AttAni> _attAnis = new List<AttAni>();

        protected UIClient g_mgr;
        public static AttAniManager singleton;

        public AttAniManager(UIClient m)
        {
            singleton = this;
            g_mgr = m;
        }

        public void AddAttAni( AttAni ani )
		{
			if( ani == null ) return;
			
			if( _attAnis.IndexOf(ani) >= 0 )
			{
				DebugTrace.add( Define.DebugTrace.DTT_ERR, "AddAttAni ani already exist!" );
				return;
			}
			
			_attAnis.Add( ani );

            ani.Start(this.g_mgr.g_netM.CurServerTimeStampMS);
		}
		
		public void RemoveAttAni( AttAni ani )
		{
			int idx =  _attAnis.IndexOf(ani);
			if( idx >= 0 )
			{
                _attAnis.RemoveAt(idx);
			}
		}
		
		//----------------------------------------------------------------------------------------
		
		//----------------------------------------------------------------------------------------
		public void process(float tmSlice)
		{
			if( _attAnis.Count == 0 ) return;
			
			float currTm = this.g_mgr.g_netM.CurServerTimeStampMS;
            for (int i = 0; i < _attAnis.Count; i++)
            {
                AttAni ani = _attAnis[i];
                if (ani != null)
                {
                    ani.Update(currTm);
                }
            }
		}
    }
}