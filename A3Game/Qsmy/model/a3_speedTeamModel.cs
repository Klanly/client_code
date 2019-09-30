using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameFramework;
using System.Collections;


namespace MuGame
{
    class a3_speedTeamModel:ModelBase<a3_speedTeamModel>
    {
       public  List<teamdata> itemTeamDataList = new List<teamdata>();


    }
    public class teamdata
    {
        public int id;
    }
}
