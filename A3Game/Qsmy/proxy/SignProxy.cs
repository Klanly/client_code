using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;


namespace MuGame
{
    class SignProxy : BaseProxy<SignProxy>
    {
        public static uint SIGN_ALLINFO = 1;
        public static uint SIGN_SIGN = 2;
        public static uint SIGN_FILL = 3;
        public bool isOneOpen = false;
        public bool isFirstGet = false;
        public bool isOneOpen_ShouChong = false;
        public Variant _variant = null;
        public SignProxy()
        {
            //addProxyListener(PKG_NAME.S2C_SIGN, onLoadSign);

        }
        public void sendMonth()
        {

            //sendRPC(PKG_NAME.S2C_SIGN, null);

        }
        public void sendDaysign()
        {
            Variant msg = new Variant();
            msg["daysign"] = 1;
            //sendRPC(PKG_NAME.S2C_SIGN, msg);
        }
       
        public void sendFillsign()
        {
            Variant msg = new Variant();
            msg["fillsign"] = 1;
            //sendRPC(PKG_NAME.S2C_SIGN, msg);
        }
        



        public void onLoadSign(Variant data)
        {

            
            if (data.ContainsKey("daysign"))
            {
                debug.Log("要签到：" + data.dump());
                dispatchEvent(GameEvent.Create(SIGN_SIGN, this, data));

            }
            if (data.ContainsKey("fillsign"))
            {
                debug.Log("要补签：" + data.dump());
                dispatchEvent(GameEvent.Create(SIGN_FILL, this, data));

            }
            if (data.ContainsKey("month_data"))
            {
                debug.Log("签到信息："+data.dump());

                if (isOneOpen_ShouChong == false && !PlayerModel.getInstance().isFirstRechange && PlayerModel.getInstance().lvl > 1)
                {
                    //InterfaceMgr.getInstance().open(InterfaceMgr.FIRST_RECHANGE);
                }
                else if (isOneOpen == false && data["today"]==0)
                {
					//if (FunctionOpenMgr.instance.checkLv(FunctionOpenMgr.SIGN, false))
					//{
					//    InterfaceMgr.getInstance().open(InterfaceMgr.SIGN);
					//    isOneOpen = true;
					//}
                }
                else
                {
                    SignProxy.getInstance().isOneOpen = true;
                }
                isOneOpen_ShouChong = true;
                if (data["today"]==0)
                {
                    isFirstGet = false;
                }
                else
                {
                    isFirstGet = true;
                }
                dispatchEvent(GameEvent.Create(SIGN_ALLINFO, this, data));
            }
            
        }


    }
}
