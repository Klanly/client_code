using System;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
    public class DispAttAni
    {
        public DispAttAni(IUIBaseControl baseCtrl)
        {
            _baseControl = baseCtrl;

            Variant ctrl = new Variant();
            ctrl._val = _baseControl;
            _attAni = new AttAni(ctrl);
        }

        private IUIBaseControl _baseControl= null;
		
		private AttAni _attAni = null;
		
		private Boolean _playing = false;

        private Action<DispAttAni> _endFun;

        public Variant userdata
        {
            get
            {
                return _attAni.userdata;
            }
            set
            {
                _attAni.userdata = value;
            }
        }
		
		public void SetFinishFun( Action<DispAttAni> fin )
		{
			_endFun = fin;
		}
		
		public IUIBaseControl baseControl
		{
            get
            {
			    return _baseControl;
            }
		}
		//----------------------------------------------------------------------------------------------
		public Boolean IsPlaying
		{
            get
			{
                return _playing;
            }
		}
		
		/**
		 *开始播放动画 
		 */
		public void Play()
		{
			if( !_playing )
			{
				_playing = true;
				AttAniManager.singleton.AddAttAni( _attAni );
				_attAni.SetFinFun( onAniStop );
			}
		}
		
		public void Stop( )
		{
			if( _playing )
			{
				_playing = false;
				AttAniManager.singleton.RemoveAttAni( _attAni );
			}
		}
		
		public void Release()
		{
			Stop();
			
			_attAni.Release();
			_baseControl = null;
		}	
		
		//----------------------------------------------------------------------------------------------
        public void AddMoveAni(Func<Variant, double, double> tweenFun, Vec2 b, Vec2 e, float tm)
		{
			Variant aniAtt;
			if( b.x != e.x )
			{
                aniAtt = GameTools.createGroup("begin",b.x,"change",(e.x-b.x),"duration",tm);
				_attAni.AddAni( tweenFun, "x", aniAtt );
			}
			if( b.y != e.y )
			{
                aniAtt = GameTools.createGroup("begin",b.y,"change",(e.y-b.y),"duration",tm);
				_attAni.AddAni( tweenFun, "y", aniAtt );
			}
		}

        public void AddSizeAni(Func<Variant, double, double> tweenFun, float b, float e, float tm, int cnt = 1)
		{	
			if( b != e )
			{
                Variant aniAtt = GameTools.createGroup("begin",b,"change",(e-b),"duration",tm);
				_attAni.AddAni( tweenFun, "scaleX", aniAtt, cnt );
				
				aniAtt = GameTools.createGroup("begin",b,"change",(e-b),"duration",tm);
				_attAni.AddAni( tweenFun, "scaleY", aniAtt, cnt );
			}		
		}

        public void AddAttAni(Func<Variant, double, double> tweenFun, String attnm, float b, float e, float tm, int cnt = 1)
		{	
			if( b != e )
			{
                Variant aniAtt = GameTools.createGroup("begin",b,"change",(e-b),"duration",tm);
				_attAni.AddAni( tweenFun, attnm, aniAtt, cnt );
			}		
		}
		public void AdjustAniAtt(String attnm, Variant att)
		{
			_attAni.AdjustAniAtt(attnm,att);
		}
		public void RemoveAttAni( String attnm )
		{
			_attAni.RemoveAniAtt( attnm );
		}	
	
		//-----------------------------------------------------------------------------------------------
		private void onAniStop( AttAni ani )
		{
			Stop();
			
			if( _endFun != null )
			{
				_endFun( this );
			}
		}
    }
}