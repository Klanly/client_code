
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
	public class GameEvent
	{
        static protected LinkedList<GameEvent> m_pool = new LinkedList<GameEvent>();
        
		private uint _type;
		private object _target; //引用数据
		private object _data;   //引用数据		
		private Boolean _GC_FLAG;//是否立即回收
		 
		private Boolean _immediately;//是否立即处理
		public Boolean immediately
		{
			get{
			
				return _immediately;
			}
		}

		public Boolean GC_FLAG
		{
			get{
			
				return _GC_FLAG;
			}
		}

        static public GameEvent alloc(uint type, object target, object data, bool gcFlag = false, bool flag=true)
        {
            if (m_pool.Count > 0)
            {
                GameEvent evt = m_pool.Last.Value;
                m_pool.RemoveLast();
                evt._type = type;
                evt._target = target;
                evt._data = data;
                evt._immediately = flag;
				evt._GC_FLAG = gcFlag;
				 
                return evt;
            }
			 
            return new GameEvent(type, target, data, flag);
        }

        static public void free(GameEvent evt)
        {
            if (evt == null)
                return;

            evt._type = 0;
            evt._target = null;
            evt._data = null;
            evt._immediately = false;

            m_pool.AddLast(evt);
        }

		public GameEvent(uint type, object target, object data, bool flag, bool gcFlag = false )
 
		{
			_type = type;
			_target = target;
			_data = data;
			_immediately = flag;
			_GC_FLAG = gcFlag;
			 
		}

		static public GameEvent Create(uint type, object target, object data, bool gcFlag = false ) 
		{
			GameEvent e = GameEvent.alloc(type, target, data, gcFlag, false );			 
			return e;
		}

		static public GameEvent Createimmedi(uint type, object target, object data, bool gcFlag = false)
		{
			GameEvent e = GameEvent.alloc(type, target, data, gcFlag, true );			 
			return e;
		}
 	
		public uint type
		{
			get { return _type; }
		}
		public object target
		{
			get { return _target; }
		}

		public Variant data
		{
			get { 
				return _data as Variant; 
			}
		}

        public object orgdata
        {
            get
            {
                return _data;
            }
        }
	}
}