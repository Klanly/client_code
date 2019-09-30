using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class NbStop:NewbieTeachItem
    {
        public static NbStop create(string[] arr)
        {
            NbStop nb = new NbStop();
            return nb;
        }
    }
}
