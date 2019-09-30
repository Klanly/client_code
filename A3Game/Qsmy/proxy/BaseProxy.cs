using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class BaseProxy<T> : GameEventDispatcher where T : class,new()  
    {
        protected SessionFuncMgr sessionFuncMgr;
        protected NetClient netClient;
        public BaseProxy()
        {
            sessionFuncMgr = SessionFuncMgr.instance;
            netClient = NetClient.instance;
        }

        virtual protected void addProxyListener(uint id, Action<Variant> handle)
        {
            if (sessionFuncMgr == null)
            {
                return;
            }

            sessionFuncMgr.addFunc(id, handle);
        }

        virtual protected void sendRPC(uint cmd, Variant v = null)
        {
            if (v == null)
                v = new Variant();
            netClient.sendRpc(cmd, v);
        }

        virtual protected void sendTPKG(uint cmd, Variant v)
        {
            netClient.sendTpkg(cmd, v);
        }

        private static T _instance;

        public static T getInstance()
        {
            if (_instance == null)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }

    }
}
