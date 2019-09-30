using System;
using System.Collections.Generic;
using Cross;
using System.Reflection;

namespace GameFramework
{
    class AttAni
    {
        public AttAni(Variant obj)
        {
            if (obj != null && obj._val is IUIBaseControl)
            {
                _obj = GameTools.createGroup("ctrl", obj._val);
            }
            else
            {
                _obj = obj;
            }
        }

        private Variant _obj = null;
		
		private Variant _anis = new Variant();
		
		private Boolean _aniAttChanged = false;

        private Action<AttAni> _updateFun;  //更新时调用的函数
        private Action<AttAni> _finFun;  //结束时调用的函数

		private float _startTm = 0;//开始时间
		private float _totalTm = 0;//总时间		
		//----------------------------------------------------------------------------------
		public Variant userdata = null;
		
		public Variant m_data
		{
            get
			{
                return _obj;
            }
		}
		
		public void Start( float tm )
		{
			_startTm = tm;		
			
			ResetObjAtt();
		}
		
		public void ResetObjAtt()
		{
            if (_anis.Count > 0)
            {
                foreach (Variant aniObj in _anis.Values)
                {
                    aniObj["ccnt"] = 0;

                    m_data[aniObj["nm"]] = aniObj["aniAtt"]["begin"];
                }
            }
			
		}
	
		//-----------------------------------------------------------------------------------
		// attnm: 属性名字 --- obj中的属性
		//tweenFun:插值算法
		//aniAtt: 插值算法的参数  必须有下列参数  其他视具体算法需求
		//			{	begin:属性开始值, 
		//				change:属性变化范围, 
		//				duration:播放一次的时间, 		
		//			}	
		//plycnt: 播放次数 -1为无限循环
		//delay:延迟播放 时间
        public void AddAni(Func<Variant, double, double> tweenFun, String attnm, Variant aniAtt, int plycnt = 1, float delay = 0)
		{
            bool isCtrl = false;
            if (m_data.ContainsKey("ctrl"))
            {
                isCtrl = true;
                Type t = m_data["ctrl"]._val.GetType();
                object[] mm = t.GetProperties();
                PropertyInfo at = t.GetProperty(attnm);
                if (at != null && at.Name != attnm)
                {
                    return;
                }
            }
			if( !isCtrl && !m_data.ContainsKey(attnm) ) return;
			
            _anis[attnm] = GameTools.createGroup("nm",attnm,"tweenFun",tweenFun,"plycnt",plycnt,"ccnt",0,"delay",delay);
            _anis[attnm]["aniAtt"] = aniAtt;
			_aniAttChanged = true;
		}
		
		public void AdjustAniAtt( String attnm, Variant att )
		{
			Variant attObj = _anis[attnm];
			if( attObj != null )
			{
				foreach( String key in att.Keys )
				{
					if( attObj.ContainsKey(key) )
					{
						_anis[key] = att[key];
					}
				}
				
				_aniAttChanged = true;
			}
		}
		
		public void RemoveAniAtt( String attnm )
		{
			if( _anis.ContainsKey(attnm) )
			{
				_anis.RemoveKey(attnm);
				
				_aniAttChanged = true;
			}
		}
		
		private void recalcTotalTm()
		{
			_totalTm = 0;
			foreach( Variant aniObj in _anis.Values )
			{
				if( aniObj["plycnt"]._int < 0 )
				{
					_totalTm = -1;
					break;
				}
			
				float tm = aniObj["plycnt"]._int * aniObj["aniAtt"]["duration"]._float + aniObj["delay"]._float;
				if( _totalTm < tm )  _totalTm = tm;
			}
		}
		
		public void Release()
		{
			_anis = new Variant();
			_obj = null;
			_finFun = null;
			_updateFun = null;
		}	
		
		//-----------------------------------------------------------------------
		public void SetFinFun( Action<AttAni> fin )
		{
			_finFun = fin;
		}

        public void SetUpdateFun(Action<AttAni> fun)
		{
			_updateFun = fun;
		}
		
		//-------------------------------------------------------------------
		private void onAniFinish()
		{
			if( _finFun != null)
			{	
				_finFun( this );
			}
		}	
		//--------------------------------------------------------------------------------
		public void Update( float currTm )
		{
			if( _startTm <= 0 ) return;
			
			if( _aniAttChanged )
			{
				recalcTotalTm();
				_aniAttChanged = false;
			}
			
			float tm;
			float lostTm = currTm-_startTm;
            if (_anis.Count > 0)
            {
                foreach (Variant aniObj in _anis.Values)
                {
                    if (aniObj["delay"]._float > 0 && aniObj["delay"]._float > lostTm) continue;

                    tm = lostTm - aniObj["delay"]._float;
                    float duration = aniObj["aniAtt"]["duration"]._float;
                    if (aniObj["plycnt"]._int > 0)
                    {
                        if (aniObj["plycnt"]._int <= aniObj["ccnt"]._int) continue;

                        if (tm >= duration)
                        {
                            aniObj["ccnt"] = (int)(tm / duration);
                            if (aniObj["plycnt"]._int <= aniObj["ccnt"]._int)
                            {//结束
                                tm = duration;
                            }
                            else
                            {
                                tm = tm - aniObj["ccnt"]._int * duration;
                            }
                        }
                    }
                    else
                    {
                        if (tm > duration)
                        {
                            tm = tm - (int)(tm / duration) * duration;
                        }
                    }
                    setValue(aniObj["nm"], (aniObj["tweenFun"]._val as Func<Variant, double, double>)(aniObj["aniAtt"], tm));
                    //m_data[aniObj["nm"]] = (aniObj["tweenFun"]._val as Func<Variant, double, double>)(aniObj["aniAtt"], tm);
                }
            }
			
			
			if( _updateFun != null )
			{
                _updateFun(this);
			}
			
			if( _totalTm > 0 && lostTm >= _totalTm )
			{
				_startTm = 0;
				onAniFinish();
			}
		}

        private void setValue(string attnm, double v)
        {
            if (m_data.ContainsKey("ctrl"))
            {
                if(attnm == "x")
                {
                    (m_data["ctrl"]._val as IUIBaseControl).x = (float)v;
                }
                if (attnm == "y")
                {
                    (m_data["ctrl"]._val as IUIBaseControl).y = (float)v;
                }
                if (attnm == "alpha")
                {
                    (m_data["ctrl"]._val as IUIBaseControl).alpha = (float)v;
                }
                if (attnm == "scale")
                {
                    //(m_data["ctrl"]._val as IUIBaseControl).scale = (float)v;
                }

            }
            else
            {
                m_data[attnm] = v;
            }
        }
    }
}