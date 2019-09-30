using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    class FindBestoModel : ModelBase<FindBestoModel>
    {
        //public int mapCount = 0;//宝图数量（用PlayerModel中的treasure_num）
        public bool Canfly = true;
       // public string nofly_txt = "当前状态不可传送";
        public string nofly_txt = ContMgr.getCont("delivery");
        public uint waitTime = 15;

        public FindBestoModel() : base()
        {
        }
    }
}
