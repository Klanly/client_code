using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
using UnityEngine;
namespace MuGame
{
    class NbLevel:NewbieTeachItem
    {
       // private int lv = 0;


        public static NbLevel create(string[] arr)
        {
            //if (param.Length != paramNum)
            //{
                
            //    return null;
            //}
               

           
            NbLevel nbLv = new NbLevel();
        //    nbLv.lv = int.Parse(param[1].ToString());

            return nbLv;
        }

        //override public bool check()
        //{
        //    return lv <= PlayerModel.getInstance().lvl;
        //}

        override public void addListener()
        {
            PlayerModel.getInstance().addEventListener(PlayerModel.ON_LEVEL_CHANGED, onHanlde);
        }

        override public void removeListener()
        {
            PlayerModel.getInstance().removeEventListener(PlayerModel.ON_LEVEL_CHANGED, onHanlde);
        }
    }
}
