using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
namespace MuGame
{
    class HeroUtils
    {
        static public Variant getHeroVar(int hid, uint iid, float x, float y)
        {
            Variant dta = new Variant();
            dta["hid"] = hid; dta["iid"] = iid; dta["x"] = x; dta["y"] = y;
            dta["level"] = 1; dta["speed"] = 10; dta["hp"] = 10000; dta["max_hp"] = 10000;
            return dta;
        }
    }
}
