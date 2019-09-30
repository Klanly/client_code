using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class VipMgr
    {

        public static int getValue(int id)
        {
            int viplv = (int)PlayerModel.getInstance().vip;
             SXML xml;
            if (viplv == 0 || PlayerModel.getInstance().isvipActive == false)
            {
                xml = XMLMgr.instance.GetSXML("vip.viplevel", "vip_level==0");
                return xml.GetNode("vt", "type==" + id).getInt("value");
            }

             xml = XMLMgr.instance.GetSXML("vip.viplevel", "vip_level==" + PlayerModel.getInstance().vip);
            return xml.GetNode("vt", "type==" + id).getInt("value");
        }

        public static int BUY_TILI_COUNT = 0;              // 体力购买次数
        public static int BUY_EXP_STAGE_COUNT = 1;         // 经验副本购买次数
        public static int REFRESH_MSHOP_COUNT = 2;            // 神秘商店刷新次数
        public static int USE_TILI_S_COUNT = 3;           // 体力丹（小）
        public static int USE_TILI_M_COUNT = 4;             // 体力丹（中）
        public static int USE_TILI_L_COUNT = 5;             // 体力丹（大）
        public static int BUY_PK_STAGE_COUNT = 6;              // 比武场购买次数
        public static int TILI_RESTORE_TIME = 7;            // 体力恢复减少（单位：秒）
        public static int LEAVE_EXP_MUL = 8;              // 离线经验倍数
        public static int PLOT_STAGE_RESET = 9;     // 剧情副本重置次数
        public static int MASTER_STAGE_RESET = 10;               // 精英副本重置次数
        public static int ONE_KEY_CLEAN = 11;              // 一键扫荡
        public static int MSHOP_SELL_ZONE = 12;             // 神秘商店栏位增加
        public static int BATTLE_HERO_COUNT = 13;              // 英雄上阵数量
        public static int LUCKDRAW_SALE_OFF = 14;              // 抽奖折扣（%）
        public static int AUTO_FIGHT = 15;              // 自动战斗
        public static int FAMILY_TREASURE = 16;         //家族宝藏拥有个数
    }
}
