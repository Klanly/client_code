using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using UnityEngine;

namespace MuGame
{
    class NbDelay : NewbieTeachItem
    {

        public static double sec;
        public static NbDelay create(string[] arr)
        {
            NbDelay nb = new NbDelay();
            sec = double.Parse(arr[1]) * 1000;
            return nb;
        }

        override public void addListener()
        {
            ConfigUtil.SetTimeout(sec, doHandle);
        }

   

        public void doHandle()
        {
            onHanlde(null);
        }

      


    }
}
