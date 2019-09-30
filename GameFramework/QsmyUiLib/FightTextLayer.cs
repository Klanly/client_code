using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
   public class FightTextLayer:Baselayer
    {
        override public float type
        {
            get
            {
                return LAYER_TYPE_FIGHT_TEXT;
            }
        }
    }
}
