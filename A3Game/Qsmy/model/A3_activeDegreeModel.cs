using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_activeDegreeModel:ModelBase<A3_activeDegreeModel>
    {
        //更新的活跃度ID 数组
        public Dictionary<int, ActiveDegreeData> activedeg_info = new Dictionary<int, ActiveDegreeData>();


    }
    public class ActiveDegreeData
    {
        public uint id;//活跃度ID
       
        public string name;//名字
        public string desc;//描述
        public uint type;//类型
        
        public uint point;//活跃点数
        public uint condition;//达成条件
        public uint value;//条件的指标
        public uint count;//进度     
        public bool received;
        //public ActiveDegreeState state;
    }
    //public enum ActiveDegreeState
    //{
    //    UNREACHED = 0,//未达成
    //    REACHED = 1,//达成未领取
    //    RECEIVED = 2,//已经领取
    //}
}
