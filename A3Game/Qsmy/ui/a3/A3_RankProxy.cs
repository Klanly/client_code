using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

namespace MuGame
{
    class A3_RankProxy:BaseProxy<A3_RankProxy>
    {

        public const uint ON_ACHIEVEMENT_CHANGE = 0;
        public const uint ON_GET_ACHIEVEMENT_PRIZE = 1;
        public const uint ON_REACH_ACHIEVEMENT = 2;

        public const uint C2S_GET_ACHIEVEMENT_PRIZE = 4;
        //34
        public static uint RANKADDLV = 5;
		public static uint RANKREFRESH = 6;

		public A3_RankProxy()
        {
            addProxyListener(PKG_NAME.C2S_A3_ACHIEVEMENT, onLoadinfos);
        }

        //获取成就奖励
        public void GetAchievementPrize(uint achievementID)
        {
            debug.Log("34_get_achievement_cmd_4_id_::" + achievementID);
            Variant msg = new Variant();

            msg["ach_cmd"] = C2S_GET_ACHIEVEMENT_PRIZE;
            msg["ach_id"] = achievementID;

            sendRPC(PKG_NAME.C2S_A3_ACHIEVEMENT, msg);
        }

        public void sendProxy(uint ach_cmd, int add_type = -1, bool display = false)
        {
            Variant msg = new Variant();
            switch (ach_cmd)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:             
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    msg["display"] = display;
                    break;
                default:
                    break;
            }

            debug.Log("get_achievement_cmd_34_::" + ach_cmd);
            msg["ach_cmd"] = ach_cmd;
            sendRPC(PKG_NAME.C2S_A3_ACHIEVEMENT, msg);
            
        }



        public void onLoadinfos(Variant data)
        {
            debug.Log("s2c_rank_achievement_::" + data.dump());
            int res = data["res"];
            if (res < 0)
            {
                Globle.err_output(res);
                return;
            }
           
            switch (res)
            {
                case 1:
                    A3_AchievementModel.getInstance().SyncAchievementDataByServer(data);
					dispatchEvent(GameEvent.Create(RANKREFRESH, this, data));
                    break;
                case 2:
                    A3_AchievementModel.getInstance().OnAchievementChangeFromServer(data);
                    dispatchEvent(GameEvent.Create(ON_ACHIEVEMENT_CHANGE, this, data));
                    break;
                case 3:
                    break;
                case 4:
                    //获得成就奖励
                    A3_AchievementModel.getInstance().OnGetAchievePrize(data);
					if (data.ContainsKey("ach_point")) {
						PlayerModel.getInstance().ach_point = data["ach_point"];
                        a3_RankModel.nowexp = data["ach_point"];
                        A3_AchievementModel.getInstance().AchievementPoint = data["ach_point"];
						dispatchEvent(GameEvent.Create(RANKREFRESH, this, data));
					}
                    dispatchEvent(GameEvent.Create(ON_GET_ACHIEVEMENT_PRIZE,this,data));
                    break;
               
                case 5:
                    debug.Log("升级成功：" + data["title"]);
                    if(data.ContainsKey("title"))
                    {
                        PlayerModel.getInstance().titileChange(data);

                        a3_RankModel.getInstance().refreinfo(data["title"], a3_RankModel.nowexp);
						if (data.ContainsKey("ach_point")) {
							PlayerModel.getInstance().ach_point = data["ach_point"];
                            a3_RankModel.nowexp = data["ach_point"];
                            A3_AchievementModel.getInstance().AchievementPoint = data["ach_point"];
							dispatchEvent(GameEvent.Create(RANKREFRESH, this, data));
						}
                        dispatchEvent(GameEvent.Create(RANKADDLV, this, data));
                    }
                    break;
                case 6:
                    debug.Log("显示或者隐藏：" + data["title_display"]._bool);
                    PlayerModel.getInstance().titleShoworHide(data);
                    break;
                case 7:
                    //成就变化数组
                    A3_AchievementModel.getInstance().OnAchievementReachChange(data);

                    dispatchEvent(GameEvent.Create(ON_REACH_ACHIEVEMENT, this, data));
                    break;
                default:
                    Globle.err_output(data["res"]);
                    break;

            };
            if (data.ContainsKey("ach_point"))
            {
                PlayerModel.getInstance().ach_point = data["ach_point"];
                A3_AchievementModel.getInstance().AchievementPoint = data["ach_point"];
                a3_RankModel.nowexp = data["ach_point"];
            }
        }
    }
}
