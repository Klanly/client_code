using System;
using System.Collections.Generic;
using Cross;
using GameFramework;


namespace MuGame
{
    class SignModel:ModelBase<SignModel>
    {
        public SignData ReadXML(int i)
        {
            SignData signdata = new SignData(); ; 
            SXML signxml = XMLMgr.instance.GetSXML("signup.signup", "signup_times==" + i);
            if (signxml != null)
            {
                    signdata.sign_day = signxml.getInt("SIGNUP_TIMES");
                    signdata.vip_lv=signxml.getString("vip_double");
                    signdata.item_id = signxml.getString("item_id");
                    signdata.num = signxml.getInt("num");
            }
            return signdata;
           
        }
    }



    public class SignData
    {
        public int sign_day;
        public string vip_lv;
        public string item_id;
        public int num;

    }
    
}
