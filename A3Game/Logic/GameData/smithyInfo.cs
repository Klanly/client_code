using System;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    public class smithyInfo : LGDataBase
    {
        public smithyInfo(muNetCleint m)
			: base(m)
		{
			
		}
        public static IObjectPlugin create(IClientBase m)
        {
            return new smithyInfo(m as muNetCleint);
        }
        override public void init()
        {//
            //this.addEventListener();
            //this.m_mgr.addEventListener();
            //this.m_mgr.addEventListener();
        }

     
    }
}
