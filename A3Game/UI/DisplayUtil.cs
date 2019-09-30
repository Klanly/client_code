using System;
using System.Collections;
using System.Collections.Generic;
using Cross;
using GameFramework;

namespace MuGame
{
    class DisplayUtil
    {
        protected muUIClient _lgClient;
        static public DisplayUtil singleton;
        public DisplayUtil(muUIClient m)
        {
            _lgClient = m;
            singleton = this;
        }
        public uint GetEqpColorUint(Variant data, Variant conf, Boolean drop = false)
        {
            uint color = 0xffffff;
            if (drop)
            {
                color = 0xcccccc;//gray
            }
            if (null == data)
            {
                return color;
            }


            int flvl = 0;
            if (data.ContainsKey("flvl"))
            {
                flvl = data["flvl"];

                if (flvl < 7)//白
                {
                    color = 0xffffff;
                }
                else//黄
                {
                    color = 0xffcc19;
                }
            }
            if ((data.ContainsKey("fp") && (data["fp"] | 61680) > 61680))
            {
                if (flvl < 7)//蓝
                {
                    color = 0x66b2ff;
                }
            }
            if ((data.ContainsKey("flag") && data["flag"] > 0))
            {
                if (2 == data["flag"] && null != conf)//当只有技能时
                {
                    if (conf.ContainsKey("skil") && flvl < 7)
                    {
                        color = 0x66b2ff;
                    }
                }
                else if (1 == data["flag"])//当有幸运时
                {
                    color = 0x66b2ff;
                }
            }
            if (IsExatt(data))//绿
            {
                color = 0x19ff80;//green
            }

            if (null != conf && conf.ContainsKey("suitid"))
            {
                color = 0x00ff00;
            }
            return color;
        }
        /**
         *卓越数量 
         */
        public int CountExatt(Variant data)
        {
            int count = 0;
            if (data.ContainsKey("veriex_cnt"))
            {
                count += data["veriex_cnt"];
            }
            if (data.ContainsKey("exatt"))
            {
                for (int i = 0; i < 32; ++i)
                {
                    if ((0x0001 << i & (data["exatt"])) != 0)
                    {
                        count++;
                    }
                }
            }
            else if (data.ContainsKey("exatt_show") && null != data["exatt_show"])
            {//随机属性
                count++;
            }
            else if (data.ContainsKey("make_att"))
            {
                Variant make_att = (_lgClient.g_gameConfM as muCLientConfig).svrItemConf.get_make_att(data["make_att"]);
                if (make_att)
                {
                    count += CountExatt(make_att);
                }
            }

            if (data.ContainsKey("ex_att_grp"))
            {
                Variant ex_att_grp = (_lgClient.g_gameConfM as muCLientConfig).svrItemConf.Get_ex_att_grp(data["ex_att_grp"]);
                if (null != ex_att_grp)
                {
                    count += ex_att_grp["mincnt"];// 目前相同 
                }
                else
                {
                    count += 1;
                }
            }
            return count;
        }
        /**
         * 是否是卓越物品
         * */
        public Boolean IsExatt(Variant data)
        {
            if (data == null)
            {
                return false;
            }
            if (data.ContainsKey("make_att") && data["make_att"] > 0)
            {
                Variant make_att = (_lgClient.g_gameConfM as muCLientConfig).svrItemConf.get_make_att(data["make_att"]);
                if (null != make_att)
                {
                    if (IsExatt(make_att))
                    {
                        return true;
                    }
                }
            }
            int exatt = 0;
            int ex_att_grp = 0;
            int veriex_cnt = 0;
            if (data.ContainsKey("exatt") && data["exatt"]!=null) int.TryParse(data["exatt"], out exatt);
            if (data.ContainsKey("ex_att_grp")) int.TryParse(data["ex_att_grp"], out ex_att_grp);
            if (data.ContainsKey("veriex_cnt")) int.TryParse(data["veriex_cnt"], out veriex_cnt);
            return exatt > 0 || ex_att_grp > 0 || veriex_cnt > 0;
        }
    }
}
