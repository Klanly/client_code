using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public class Window : Baselayer, IBgLayerUI
    {
        override public bool showBG
        {
            get { return true; }
        }
        public bool OpenAni = false;
        override public bool openAni
        {
            get { 
                if (OpenAni)
                     return true;
                  else 
                     return false; 
            }
        }
    }
}
