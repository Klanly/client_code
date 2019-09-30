using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;


namespace MuGame
{
    class SettingsProxy : BaseProxy<SettingsProxy>
    {
        public static uint CHANGE_NAME = 1; 
        public Variant _variant = null;
        public SettingsProxy()
        {
            addProxyListener(PKG_NAME.S2C_MODIFY_NAME_RES, getName);
        }

        public void getName(Variant data)
        {
            int res = data["res"];
            if (res > 0)
            {
                PlayerModel.getInstance().name = data["name"];
            }         
            dispatchEvent(GameEvent.Create(CHANGE_NAME, this, data));
        }
        public void SendModifName(string name)
        {
            Variant msg = new Variant();
            msg["name"] = name;
            sendRPC(PKG_NAME.C2S_MODIFY_NAME, msg);
        }
    }
}
