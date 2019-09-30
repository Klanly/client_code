using Cross;
using UnityEngine;

namespace MuGame
{
    public interface IPlotformSDK
    {
        void FrameMove();
        void Add_moreCmdInfo(string info, string jstr);
        void Call_Cmd(string cmd, string info = null, string jstr = null, bool waiting = true);
        void Cmd_CallBack(Variant v);
        int isinited { get; }
    }



    public class PlotformBaseSDK : IPlotformSDK
    {
        public void FrameMove()
        {

        }

        public void Add_moreCmdInfo(string info, string jstr)
        {
            debug.Log("Base SDK (Add_moreCmdInfo):" + info);
        }

        public void Call_Cmd(string cmd, string info = null, string jstr = null, bool waiting = true)
        {
            debug.Log("Base SDK (Call_Cmd):" + cmd + " " + info + " " + jstr);
        }

        public void Cmd_CallBack(Variant v)
        {
            debug.Log("Base SDK (Cmd_CallBack):" + v.dump());
        }

        public int isinited
        {
            get { return 1; }
        }
    }
}
