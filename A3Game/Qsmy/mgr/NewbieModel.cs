using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    class NewbieModel
    {
        Transform transCon;
        GameObject goBg;
        public NewbieModel()
        {
            transCon = GameObject.Find("newbieLayer").transform;
            goBg = GameObject.Instantiate(U3DAPI.U3DResLoad<GameObject>("newbiebg")) as GameObject;



        }


        private static NewbieModel instance;
        public static NewbieModel getInstance()
        {
            if (instance == null)
                instance = new NewbieModel();
            return instance;
        }
    }
}
