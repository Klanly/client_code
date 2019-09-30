using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public class GameJoyUI : Baselayer
    {
        override public float type
        {
            get
            {
                return LAYER_TYPE_GAMEJOY;
            }
        }
    }
}
