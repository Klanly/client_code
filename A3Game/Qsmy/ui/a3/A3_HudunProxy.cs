using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Cross;

namespace MuGame
{
    class A3_HudunProxy : BaseProxy<A3_HudunProxy>
    {
        HudunModel HudunModel = HudunModel.getInstance();
        public A3_HudunProxy()
        {
            addProxyListener(PKG_NAME.C2S_LVLMIS_PRIZE, onLoadinfo);
        }

        public void sendinfo(int val)
        {
            Variant msg = new Variant();
            msg["op"] = val;
            sendRPC(PKG_NAME.C2S_LVLMIS_PRIZE, msg);
        }
        public void sendinfo(int val, int auto)
        {
            Variant msg = new Variant();
            msg["op"] = val;
            msg["auto"] = auto;
            sendRPC(PKG_NAME.C2S_LVLMIS_PRIZE, msg);
        }
        bool isUplvl = false;
        void onLoadinfo(Variant data)
        {
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(data["res"]);
                return;
            }
            else
            {
                debug.Log(data.dump());
                if (data["level"] > HudunModel.Level)
                {
                    if (a3_hudun._instance)
                    {
                        a3_hudun._instance.AniUpLvl();
                    }
                    isUplvl = true;
                }
                int oldcount = HudunModel.NowCount;
                HudunModel.Level =data["level"];
                HudunModel.NowCount =data["holy_shield"];
                isshow = true;
                //HudunModel.GetMaxCount(HudunModel.Level) = data["max_holy_shield"];
                if (data["auto"] == 0) { HudunModel.is_auto = false; }
                if (a3_hudun._instance)
                {
                    if (isUplvl)
                    {
                        a3_hudun._instance.updata_hd(0);
                        isUplvl = false;
                    }
                    else
                    {
                        a3_hudun._instance.updata_hd(oldcount);
                    }
                }
            }
        }
        //自动充能
        bool isshow = true;
        public void Add_energy_auto()
        {
            if (HudunModel.is_auto)
            {
                if (HudunModel.Level <= 0) {  }
                else
                {
                    if (HudunModel.NowCount >= HudunModel.GetMaxCount(HudunModel.Level))
                    {
                       
                    }
                    else
                    {
                        if (HudunModel.OnMjCountOk_auto(HudunModel.hdData[HudunModel.Level].addcount))
                        {
                            sendinfo(2);
                            flytxt.instance.fly(ContMgr.getCont("a3_hudun_cncg"), 1);
                        }
                        else
                        {
                            if(isshow)
                            {
                                flytxt.instance.fly(ContMgr.getCont("a3_hudun_nomj"), 1);
                                isshow = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
