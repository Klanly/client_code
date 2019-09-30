using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
namespace MuGame
{
    class NbClick : NewbieTeachItem
    {
        //private string clickName;
        //private bool clicked = false;
        public static NbClick create(string[] arr)
        {
            //if (param.Length != paramNum)
            //{
            //    return null;
            //}

            NbClick nbclick = new NbClick();
          //  nbclick.clickName = param[1].ToString();

            return nbclick;
        }

        //override public bool check()
        //{
        //    return clicked;
        //}

        override public void addListener()
        {
           MouseClickMgr.instance.addEventListener(MouseClickMgr.EVENT_TOUCH_UI, onHanlde);
        }

        override public void removeListener()
        {
            MouseClickMgr.instance.removeEventListener(MouseClickMgr.EVENT_TOUCH_UI, onHanlde);
        }

        //void onCLick(GameEvent e)
        //{
        //    GameObject go = e.orgdata as GameObject;
        //    if (go == null)
        //        return;
        //    if (go.name == clickName)
        //    {
        //        clicked = true;
        //        doit();
        //        if (addedLinstener)
        //            removeListener();

        //        //if (l.Contains(this))
        //        //    l.Remove(this);
        //    }
        //}
    }
}
