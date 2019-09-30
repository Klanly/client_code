using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;
namespace MuGame
{
    class GeneralProxy : BaseProxy<GeneralProxy>
    {
        public GeneralProxy()
            : base()
        {
            addProxyListener(PKG_NAME.S2C_ON_CLIENT_CONFIG, onConfig);
            addProxyListener(PKG_NAME.S2C_ON_SOMETHING_TODO, onSomethingTodo);
            addProxyListener(PKG_NAME.S2C_GOD_LIGHT, onGodLightActive);
            addProxyListener(PKG_NAME.S2C_ON_PING, onGetPing);
            //addProxyListener(PKG_NAME.S2C_PK_STATE_CHANGE, onPkStateChange);
        }

        public void sendGetPing()
        {
            sendRPC(PKG_NAME.C2S_ON_PING);
        }
        void onGetPing(Variant d)
        {
            if (a3_lowblood.instance != null)
                a3_lowblood.instance.refreshLinkTime();
        }
        /**
         *获得客户端配置  
         * 
         */
        public void sendGetClientConfig()
        {
           // sendRPC(PKG_NAME.S2C_ON_CLIENT_CONFIG);
        }

        /**
        * 改变PK状态
        * */
        public void SendPKState(bool changePk)
        {
            Variant d = new Variant();
            d["tp"] = 1;
            d["pk_state"] = changePk ? 1 : 0;

            //sendRPC(PKG_NAME.S2C_PK_STATE_CHANGE, d);
        }


  
      //<if name="pk_state">
      //  <!---->
      //  <v name="pk_state" type="uint" bit="3" />
      //</if>
      //<if name="defend">
      //  <!---->
      //  <v name="defend" type="bool"/>
      //  <!---->
      //  <v name="cid" type="uint" bit="24" />
      //</if>
      //<if name="be_defend">
      //  <!---->
      //  <v name="be_defend" type="bool"/>
      //  <!---->
      //  <v name="cid" type="uint" bit="24" />
      //</if>
   
        void onPkStateChange(Variant d)
        {
            if (d.ContainsKey("pk_state"))
            {
                bool pkState = d["pk_state"] == 1;
                PlayerModel.getInstance().pkState = pkState;
                //if (herohead.instance != null)
                //    herohead.instance.refreshPkState(pkState);
            }

            //if (d.ContainsKey("defend"))
            //{
            //    bool defend = d["defend"] == 1;
            //    PlayerModel.getInstance().inDefendArea = defend;
            //    pk_notify.show();
            //}

        }
        void onSomethingTodo(Variant data)
        {
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
            if (res == 1)
            {
                PlayerModel.getInstance().isFirstRechange = true;
                //if (minimap.instance)
                //{
                //    minimap.instance.doCloseFirstRecharge();
                //}
            }
        }
        public void onConfig(Variant data)
        {

          //  SkillModel.getInstance().initQuickBar(data);
           // dispatchEvent(GameEvent.Create(PKG_NAME.S2C_ON_CLIENT_CONFIG, this, data));
        }

        //第一次收到协议时界面还没初始化，这里保存下数据
        public bool active_open = false;
        public uint active_left_tm = 0;
        public void onGodLightActive(Variant data)
        {
            active_open = data["open"]._bool;
            active_left_tm = data["left_tm"];

            if (a3_liteMinimap.instance)
            {
                a3_liteMinimap.instance.showActiveIcon(active_open, active_left_tm);
            }
        }
    }
}
