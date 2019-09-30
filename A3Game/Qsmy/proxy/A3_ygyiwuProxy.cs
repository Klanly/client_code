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
    class A3_ygyiwuProxy : BaseProxy<A3_ygyiwuProxy>
    {

        public static uint EVENT_YWINFO = 0;
        public static uint EVENT_ZHISHIINFO = 1;
        public A3_ygyiwuProxy():base()
        {
            addProxyListener(PKG_NAME.S2C_SETUP_SKILL, onGetYGexp);
        }  


        public void  SendYGinfo( uint val)
        {
            Variant msg = new Variant();
            msg["op"] = val;
            sendRPC(PKG_NAME.S2C_SETUP_SKILL, msg);
        }
        
        void onGetYGexp(Variant data)
        {
            debug.Log("远古经验"+data.dump ());
            int res = data["res"];
            if (res == 1)
            {
                a3_ygyiwuModel.getInstance().nowGod_id = data["god_remains_id"];
                a3_ygyiwuModel.getInstance().nowGodFB_id = data["g_levelid"];
                PlayerModel.getInstance().accent_exp = data["exp"];
                a3_ygyiwuModel.getInstance().nowPre_id = data["king_remains_id"];
                a3_ygyiwuModel.getInstance().nowPreFB_id = data["k_levelid"];
                a3_ygyiwuModel.getInstance().nowPre_needupLvl = data["zhuan"];
                a3_ygyiwuModel.getInstance().nowPre_needLvl = data["lvl"];

                a3_ygyiwuModel.getInstance().loadList();
                dispatchEvent(GameEvent.Create(EVENT_YWINFO, this, data));

                if (a3_liteMinimap.instance)
                {
                    a3_liteMinimap.instance.refreshYGexp();
                }
            }
            else if (res == 2)
            {
                //知识魔典信息
                a3_ygyiwuModel.getInstance().yiwuLvl = data["remains_lvl"];
                a3_ygyiwuModel.getInstance().studyTime = data["time_left"];
                //a3_ygyiwuModel.getInstance().studyTime_all = data["need_time"];
                dispatchEvent(GameEvent.Create(EVENT_ZHISHIINFO,this,data));

            }
            else if (res == 3)
            {
                //开始研究
                a3_ygyiwuModel.getInstance().studyTime = data["time"];//研究需要的时间

                if (a3_ygyiwu.instan)
                {
                    a3_ygyiwu.instan.ref_StudyBtn();
                }
            }
            else if (res == 4)
            {
                //当前遗物经验
                PlayerModel.getInstance().accent_exp = data["remains_exp"];
                if (a3_liteMinimap.instance)
                {
                    a3_liteMinimap.instance.refreshYGexp();
                }

                if (a3_expbar.instance )
                {
                    a3_expbar.instance.showiconHit();
                }
            }
            else if (res == 5)
            {
                //副本挑战成功后神王遗物经验 遗物id 副本等级
                PlayerModel.getInstance().accent_exp = data["remains_exp"];
                a3_ygyiwuModel.getInstance().nowGod_id = data["god_remains_id"];
                a3_ygyiwuModel.getInstance().nowGodFB_id = data["g_levelid"];
                if (a3_liteMinimap.instance)
                {
                    a3_liteMinimap.instance.refreshYGexp();
                }
            }
            else if (res == 6)
            {
                //副本挑战成功后人王遗物id 副本等级 下个遗物需要的等级转数
                a3_ygyiwuModel.getInstance().nowPre_id = data["king_remains_id"];
                a3_ygyiwuModel.getInstance().nowPreFB_id = data["k_levelid"];
                a3_ygyiwuModel.getInstance().nowPre_needupLvl = data["zhuan"];
                a3_ygyiwuModel.getInstance().nowPre_needLvl = data["lvl"];

                if (a3_expbar.instance)
                {
                    a3_expbar.instance.showiconHit();
                }
                dispatchEvent(GameEvent.Create(EVENT_YWINFO, this, data));
            }

        }



    }
}
