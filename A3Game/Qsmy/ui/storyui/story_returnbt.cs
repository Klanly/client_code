using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine.UI;
using UnityEngine;
namespace MuGame
{
    class story_returnbt : StoryUI
    {
        public override void init()
        {
            alain();
        }

        public override void onShowed()
        {
            base.onShowed();
            this.getEventTrigerByPath("bt").onClick = onClick;
        }

        public override void onClosed()
        {
            base.onClosed();
            this.getEventTrigerByPath("bt").onClick = onClick;
        }

        void onClick(GameObject go)
        {
            //gameST.REQ_STOP_PLOT();
        }
    }
}
