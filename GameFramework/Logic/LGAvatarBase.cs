
using System;
using System.Collections;
using System.Collections.Generic;
using Cross;

namespace GameFramework
{
	public class  LGAvatarBase : lgGDBase
	{
        public static int ROLE_TYPE_ROLE=0;
        public static int ROLE_TYPE_MONSTER = 1;
        public static int ROLE_TYPE_PLAYER = 2;
        public static int ROLE_TYPE_USER = 3;
        public static int ROLE_TYPE_HERO = 4;

        protected int _roletype = ROLE_TYPE_ROLE;
        virtual public int roleType
        {
            get { return _roletype; }
        }


		protected float _ori = 0;
		public LGAvatarBase( gameManager m):base(m)
		{
		}
        virtual public float x
        {
            get;
            set;
        }
        virtual public float y
        {
            get;
            set;
        }
		virtual public float lg_ori_angle
		{
			get;
			set;
		}

		virtual public uint octOri
		{
			get{	
				return (uint)( (_ori + 0.5 * Math.PI) / (Math.PI*0.25) );
			}
			set{
				_ori = (float)( value * (Math.PI*0.25) -0.5 *  Math.PI );
			}
		} 

	}
}
  
 
 
  