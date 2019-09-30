using System;

namespace GameFramework
{
    public class processStruct : IProcess
    {
        public processStruct(Action<float> fun, string processName = "", bool pause = false, bool destory = false)
        {
            _pause = pause;
            _destory = destory;
            _update = fun;
            _processName = processName;
        }
        static public processStruct create(Action<float> fun = null, string processName = "", bool pause = false, bool destory = false)
        {
            return new processStruct(fun, processName, pause, destory);
        }
        private bool _pause;
        private bool _destory;
        private string _processName;
        private Action<float> _update;
        public bool destroy
        {
            get
            {
                return _destory;
            }
            set
            {
                _destory = value;
            }
        }
        public bool pause
        {
            get
            {
                return _pause;
            }
            set
            {
                _pause = value;
            }
        }
        public string processName
        {
            get
            {
                return _processName;
            }
            set
            {
                _processName = value;
            }

        }

        public Action<float> update
        {
            set
            {
                _update = value;
            }
        }
        public void updateProcess(float tmSlice)
        {
            _update(tmSlice);
        }
    }
}
