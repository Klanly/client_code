using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using Cross;


namespace MuGame
{
    class A3_signProxy : BaseProxy<A3_signProxy>
    {
        public static uint SIGNINFO = 1001;
        public static uint AllREPARISIGN = 1002;
        public static uint SIGNorREPAIR = 1003;
        public static uint ACCUMULATE = 1004;
        public static uint SIGNINFO_YUEKA = 1005;
        public static uint TOTAL_SIGNUP = 5;
        public A3_signProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_SIGN, onLoadSign);
        }
        //c2s
        public void sendproxy(int tp, int day , uint tolal_id = 0)
        {

            Variant msg = new Variant();
            msg["tp"] = tp;
            switch (tp)
            {
                case 1://签到状态
                    break;
                case 2://每日签到
                    break;
                case 3://补签
                    msg["day"] = day;
                    break;
                case 4://补签所有
                    break;
                case 5:
                    msg["tolal_idx"] = tolal_id;
                    break;
            }
            sendRPC(PKG_NAME.C2S_A3_SIGN, msg);
        }


        public bool checksign() {
            if (haveyueka == false) return false;
            if (canqd) { return true; }
            if (canget) { return true; }
            return false;
        }

        bool haveyueka = false;
        bool canqd = true;
        bool canget = false;
        public  int yueka = 0;

        //s2c
        void onLoadSign(Variant data)
        {
            debug.Log("签到的信息：" + data.dump());

            if (data["res"] == -4003|| data["res"] == -5202 || data["res"] == -5201 ) {
                Globle.err_output(data["res"]);
            }

            //if (data.ContainsKey("qd_days")) {

            //    debug.Log("今天"+ DateTime.Now.Day);


            //}

            if (data["res"] == 1)
            {
                haveyueka = false;
                canqd = true;
                canget = false;
                if (data["yueka"] > 0) { haveyueka = true; }
                else haveyueka = false;

                yueka = data["yueka"]._int;

                foreach (int j in data["qd_days"]._arr) {
                    if (j == DateTime.Now.Day) {
                        canqd = false;
                        break;
                    }
                }
                int num = data["qd_days"].Count;
                List <SXML> xml = XMLMgr.instance.GetSXMLList("signup_a3.total");
                List<int> gif = new List<int>();
                List<int> gif_get = new List<int>();
                foreach (SXML one in xml)
                {
                    if (one.getInt ("total") <= num) {
                        gif.Add(one.getInt("total"));
                    }
                }
                foreach (int j in data["count_type"]._arr)
                {
                    gif_get.Add(j);
                }

                foreach (int i in gif) {
                    if (gif_get.Contains(i))
                        continue;
                    else {
                        canget = true;
                        break;
                    }
                }
                InterfaceMgr.doCommandByLua("a1_low_fightgame.canget_yueka", "ui/interfaces/low/a1_low_fightgame", checksign());
            }

            if (data["res"] == -1100)
            {
                flytxt.instance.fly(ContMgr.getCont("nullrew_toget"));
            }
            if(data.ContainsKey("yueka"))
            {

                dispatchEvent(GameEvent.Create(SIGNINFO_YUEKA, this, data));
            }
            if (data.ContainsKey("yueka_tm"))
            {
               
                dispatchEvent(GameEvent.Create(SIGNINFO, this, data));
               // IconAddLightMgr.getInstance().showOrHideFire("refreshSign",data);
                //InterfaceMgr.doCommandByLua("a1_low_fightgame.refreshSign", "ui/interfaces/low/a1_low_fightgame", data);

            }
            if (data.ContainsKey("daysign"))
            {
                debug.Log("签到或单个补签：" + data.dump());
                dispatchEvent(GameEvent.Create(SIGNorREPAIR, this, data));
               // IconAddLightMgr.getInstance().showOrHideFire("singorrepair",data);
                //InterfaceMgr.doCommandByLua("a1_low_fightgame.singorrepair", "ui/interfaces/low/a1_low_fightgame", data);
            }
            if (data.ContainsKey("fillsign_all"))
            {
                debug.Log("一键补签：" + data.dump());
                dispatchEvent(GameEvent.Create(AllREPARISIGN, this, data));
            }
            //if (data.ContainsKey("total_signup"))
            //{
            //    debug.Log("累积奖励：" + data.dump());
            //    dispatchEvent(GameEvent.Create(ACCUMULATE, this, data));
            //}
            if (data["res"] == 5)
            {
                if (data.ContainsKey("total_signup")) {
                    debug.Log("累积奖励领取：" + data.dump());
                    dispatchEvent(GameEvent.Create(TOTAL_SIGNUP, this, data));
                }

            }
        }
    }
}
