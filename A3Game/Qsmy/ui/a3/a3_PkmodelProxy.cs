using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class a3_PkmodelProxy : BaseProxy<a3_PkmodelProxy>
    {
        public a3_PkmodelProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_PKMODEL, onLoadInfo);
            addProxyListener(PKG_NAME.C2S_A3_WASHREDNAME, onLoadWashredname);
        }


        public void sendProxy(int pkstate)
        {
            Variant msg = new Variant();
            msg["pk_state"] = pkstate;
            sendRPC(PKG_NAME.C2S_A3_PKMODEL, msg);
        }
        public void sendWashredname(int moneytype)
        {
            Variant msg = new Variant();
            msg["tp"] = moneytype;
            sendRPC(PKG_NAME.C2S_A3_WASHREDNAME, msg);
        }


        public void onLoadInfo(Variant data)
        {
            debug.Log("pk模式的信息："+data.dump());
            if (data.ContainsKey("pk_state"))
            {
                PlayerModel.getInstance().now_pkState = data["pk_state"];
                switch (PlayerModel.getInstance().now_pkState)
                {
                    case 0:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_PEACE;
                        break;
                    case 1:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_PKALL;
                        PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().cid;
                        PlayerModel.getInstance().m_unPK_Param2 = PlayerModel.getInstance().cid;
                        break;
                    case 2:
                        PlayerModel.getInstance().pk_state = PK_TYPE.PK_TEAM;
                        PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().teamid;
                        PlayerModel.getInstance().m_unPK_Param2 = PlayerModel.getInstance().clanid;
                        break;
                    //case 3:
                    //    PlayerModel.getInstance().pk_state = PK_TYPE.PK_LEGION;
                    //    PlayerModel.getInstance().m_unPK_Param = PlayerModel.getInstance().clanid;
                    //    break;
                    //case 4:
                    //    PlayerModel.getInstance().pk_state = PK_TYPE.PK_HERO;
                    //    //？？？
                    //    break;
                }
                if(a3_pkmodel._instance)
                a3_pkmodel._instance.ShowThisImage(data["pk_state"]);
                if (SelfRole.s_LockFX.gameObject != null)
                {
                    PkmodelAdmin.RefreshShow(SelfRole._inst.m_LockRole);
                }

                InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modPkState", "model/PlayerModel", PlayerModel.getInstance().now_pkState, true);

                InterfaceMgr.getInstance().close(InterfaceMgr.A3_PKMODEL);
                NewbieModel.getInstance().hide();
            }

        }
        public void onLoadWashredname(Variant data)
        {


        }
       
    }



}
