using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;

namespace MuGame
{
    /// <summary>
    /// 图片闪光提示
    /// </summary>
    /*1:商城(√)
        //*每次上线亮，打开后关                                 */
    /*2:占卜（√）
        *拥有免费的占卜次数时亮，没有时关                     */
    /*3:兑换(√)
        //*拥有可兑换次数时亮，打开界面时关                         */
    /*4:月卡(√)
        *当天未签到时亮，签过后关                             */
    /*5:活跃(√)
        *上线有未完成的活跃选项亮，打开后关
        *有可以领取的活跃奖励时亮，领取后关                   */
    /*6:神赐(√)
        *活动开启亮，结束关                                   */
    /*7:首冲(√)
        *未首冲亮，打开界面后关                                   */   
    /*8:福利(√)
        *又可领取的奖励亮，没有时关                           */
    /*9:副本(不加了)
        *拥有可进入的副本次数时亮，没有时关                   */
    /*10:活动(√)
        *竞技场处于开启状态， 
        *上古宝箱开启，
        *藏宝图活动处于开启状态，                             */
    /*11:首领(√)
        * 有野外精英怪(存在且未被杀害)，
        * 世界Boss活动处于开启阶段且还有Boss存活              */


    class IconAddLightMgr
    {
        public static IconAddLightMgr _instance;
        public static IconAddLightMgr getInstance()
        {
            if (_instance == null)
                _instance = new IconAddLightMgr();
            return _instance;
        }

        string path = "ui/interfaces/low/a1_low_fightgame";
        string str  = "a1_low_fightgame.";
        string str1 = "a1_low_fightgame.";
        public void showOrHideFire(string way, Variant data)
        {
            InterfaceMgr.doCommandByLua(str+way, path, data);
        }
        public void showOrHideFires(string way, Variant data)
        {
            InterfaceMgr.doCommandByLua(str1 + way, path, data);
        }



    }
}
