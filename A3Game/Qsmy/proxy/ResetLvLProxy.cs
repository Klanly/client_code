using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class ResetLvLProxy : BaseProxy<ResetLvLProxy>
    {
        public static uint EVENT_RESETLVL = 9101;
        public static uint EVENT_SHOWRESETLVL = 9102;


        public ResetLvLProxy()
            : base()
        {
            addProxyListener(PKG_NAME.S2C_GET_MERIS_RES, onGetMerisRes);
        }
        public void sendResetLvL()
        {
            sendRPC(PKG_NAME.S2C_GET_MERIS_RES);
        }

        void onGetMerisRes(Variant data)
        {

            if (data["res"] == 1)
            {
                debug.Log(data.dump());
                Variant resetData = new Variant();
                resetData["zhuan"] = data["zhuan"];
                resetData["att_pt"]=data["att_pt"];
                if (data.ContainsKey("lvl")) resetData["lvl"] = data["lvl"];
                PlayerModel.getInstance().lvUp(resetData);
                PlayerModel.getInstance().up_lvl = data["zhuan"]._uint;
                PlayerModel.getInstance().pt_att = data["att_pt"]._int;
                PlayerModel.getInstance().lvl = data["lvl"] == null ? PlayerModel.getInstance().lvl : data["lvl"]._uint;
                if (data.ContainsKey("exp")) PlayerModel.getInstance().exp = data["exp"];

                resetLvL();
                Variant _data = new Variant();
                if (data.ContainsKey("lvl"))
                    data["lvl"] = data["lvl"];
                if (data.ContainsKey("zhuan"))
                    data["zhuan"] = data["zhuan"];
                InterfaceMgr.doCommandByLua("PlayerModel:getInstance().modInfo", "model/PlayerModel", _data);
                dispatchEvent(GameEvent.Create(EVENT_RESETLVL, this, resetData));
            }
            else
            {
                Globle.err_output(data["res"]); 
            }

        }
        public void resetLvL()
        {
            bool isCanReset = checkResetLvL(PlayerModel.getInstance());
            Variant showData = new Variant();
            showData["show"] = isCanReset;
            dispatchEvent(GameEvent.Create(EVENT_SHOWRESETLVL, this, showData));
        }
        public Boolean checkResetLvL(PlayerModel pm)//检测是否可以转生
        {
            int pp = pm.profession;
            uint pl = pm.lvl;
            uint pz = pm.up_lvl;
            uint exp = pm.exp;
            return isCanResetLvL(pp,pl, pz, exp);
        }

        private bool isCanResetLvL(int pp, uint pl, uint pz, uint exp)
        {
            uint needExp = ResetLvLModel.getInstance().getNeedExpByCurrentZhuan(pp,pz);
            uint needLvL = ResetLvLModel.getInstance().getNeedLvLByCurrentZhuan(pp,pz);

            if (needLvL > pl) return false;
            if (pz >= 10) return false;//10转之后无法再次转生

            //if (needExp > exp) return false;//TODO:暂时不做判断

            return true;
        }
    }
}
