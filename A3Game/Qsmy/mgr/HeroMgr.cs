using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class HeroMgr : lgGDBase, IObjectPlugin
    {
         public static HeroMgr instacne;
         public HeroMgr(gameManager m)
             : base(m)
		{
            instacne = this;
		} 
	
		 
		
    }
}
