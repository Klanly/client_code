using GameFramework;
using Cross;

namespace MuGame
{
    class A3_SmithyProxy : BaseProxy<A3_SmithyProxy>
    {
        public static readonly uint ON_SMITHYOPT = 1;
        public static readonly uint ON_SMITHYEXPCHANGE = 2;
        public static readonly uint ON_SMITHYDATACHANGED = 3;        
        public A3_SmithyProxy():base()
        {
            addProxyListener(PKG_NAME.S2C_FORGE_OPT, OnSmithyOpt);        
        }
        void OnSmithyOpt(Variant v)
        {
            debug.Log("铁匠铺协议:::::" + v.dump());
            int op = v["res"];
            switch (op)
            {
                case 1:
                    this.dispatchEvent(GameEvent.Create(A3_SmithyProxy.ON_SMITHYDATACHANGED, this, v));
                    break;
                default:
                    if (op < 0)
                        flytxt.instance.fly(ContMgr.getError(v["res"])); 
                    break;
            }
        }
        public void SendMake(uint tpid, uint way = 1, int num = 1)
        {
            Variant data = new Variant();
            data["op"] = 3;
            data["id"] = tpid;
            data["way"] = way;
            data["num"] = num;
            debug.Log("发送打造装备消息"+data.dump());
            sendRPC(PKG_NAME.C2S_FORGE_OPT, data);
        }

        public void SendMakeByScroll(int num = 1)
        {
            Variant data = new Variant();
            data["op"] = 4;
            data["num"] = num;
            debug.Log("发送卷轴打造装备消息" + data.dump());
            sendRPC(PKG_NAME.C2S_FORGE_OPT, data);
        }
        public void SendChooseLearn(uint partId)
        {
            debug.Log("发送学习专精消息");
            Variant data = new Variant();
            data["op"] = 1;
            data["type"] = partId;
            sendRPC(PKG_NAME.C2S_FORGE_OPT, data);            
        }
        public void SendRefresh()
        {
            Variant data = new Variant();
            data["op"] = 2;
            sendRPC(PKG_NAME.C2S_FORGE_OPT, data);
        }
        public void SendRelearn(int type,int costWay)
        {
            Variant data = new Variant();
            data["op"] = 5;
            data["cost"] = costWay;
            data["type"] = type;
            debug.Log("发送重学消息:" + data.dump());
            sendRPC(PKG_NAME.C2S_FORGE_OPT, data);
        }
    }
}
