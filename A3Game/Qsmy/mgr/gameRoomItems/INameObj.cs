using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MuGame
{
    public interface INameObj
    {
        UnityEngine.Vector3 getHeadPos();

        string roleName { get; set; }
        int curhp { get; set; }
        int maxHp { get; set; }
        int title_id { get; set; }
        bool isactive { get; set; }
        int rednm { get; set; }

        int spost_lvlsideid { get; set; }
        uint hidbacktime { get; set; }
    }
}
