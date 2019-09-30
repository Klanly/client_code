using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    public class FunctionItem
    {
        private static FunctionItem instance;
        public static FunctionItem Instance
        {
            get
            {
                if (instance == null)
                    instance = new FunctionItem();
                return instance;
            }
            set { instance = value; }
        }
        public List<int> haveOpenId = new List<int>();
        public void doOpen(bool showAni = false)
        {
            if (opened)
                return;
            //if (flytxt.instance!=null)
            //flytxt.instance.fly("新功能开启，停留5秒飞到对应位置，播放特效。");

            if (haveOpenId.Contains(id))
                return;

            if (showAni && show)
            {
                if (a3_funcopen.instance != null)
                    a3_funcopen.instance.refreshInfo(id, pos_x, pos_y);
            }

            haveOpenId.Add(id);

            //if (a3_liteMinimap.instance != null)
            //{               
            //    if (id == a3_liteMinimap.instance?.func_id)
            //    {
            //        a3_liteMinimap.instance.fun_i++;
            //        a3_liteMinimap.instance.function_open(a3_liteMinimap.instance.fun_i);
            //    }
            //}
            switch (id)
            {
                case 1://PK模式切换
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.OpenPk", "ui/interfaces/low/a1_low_fightgame");
                    break;
                //case 22://自动战斗
                //    if (expbar.instance)
                //        expbar.instance.autifight.hideLock();
                //      //  skillbar.instance.doOpenAutoFight();
                //    break;
                case 2://日常任务
                    if (a3_task.instance)
                        a3_task.instance.OpenDailyTask();
                    break;
                case 3://拍卖行
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenAuction();
                    break;
                case 4://成就
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenAchievement();
                    break;
                case 5://召唤兽
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenSummon();
                    break;
                case 6://技能
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenSkill();
                    break;
                case 7://翼灵
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenSWING_PET();
                    break;
                case 8://灵宠
                       //if (a3_yiling.instance)
                       //	a3_yiling.instance.OpenPet();
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenPET();
                    break;
                case 9://飞翼
                    if (a3_yiling.instance)
                        a3_yiling.instance.OpenWing();
                    break;
                case 10://活动
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.OpenActive", "ui/interfaces/low/a1_low_fightgame");
                    //if (a3_liteMinimap.instance)
                    //    a3_liteMinimap.instance.OpenActive();
                    break;
                case 11://风神王座
                    //a3_counterpart.instance?.OpenFSWZ();
                    break;
                case 12://魔炼之地
                    if (a3_active.instance)
                        a3_active.instance.OpenMLZD();
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenHallow();
                    break;
                case 13://金币副本
                    //a3_counterpart.instance?.OpenGold();
                    break;
                case 14://经验副本
                        //if (a3_active.instance)
                        //a3_active.instance.OpenExp();
                    break;
                case 15://召唤兽乐园
                    if (a3_active.instance)
                        a3_active.instance.OpenSummon();
                    break;
                case 16://世界Boss
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.openElite", "ui/interfaces/low/a1_low_fightgame");
                    break;
                case 17://魔物猎人
                    if (a3_active.instance)
                        a3_active.instance.OpenMWLR();

                    break;
                case 18://装备
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenEQP();
                    break;
                case 19://装备强化
                    if (a3_equip.instance)
                        a3_equip.instance.OpenQH();
                    break;
                case 20://装备重铸
                    if (a3_equip.instance)
                        a3_equip.instance.OpenCZ();
                    break;
                case 21://装备传承
                    if (a3_equip.instance)
                        a3_equip.instance.OpenCC();
                    break;
                case 22://装备进阶
                    if (a3_equip.instance)
                        a3_equip.instance.OpenJJ();
                    break;
                case 23://装备宝石
                    if (a3_equip.instance)
                        a3_equip.instance.OpenBS();
                    break;
                case 24://装备追加
                    if (a3_equip.instance)
                        a3_equip.instance.OpenZJ();
                    break;
                case 25://魔盒
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.OpenMH", "ui/interfaces/low/a1_low_fightgame");
                    //if (a3_liteMinimap.instance)
                    //    a3_liteMinimap.instance.OpenMH();
                    break;
                case 30:
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenHUDUN();
                    break;

                case 33:
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.OpenFB", "ui/interfaces/low/a1_low_fightgame");
                    break;
                case 35:
                    if (a3_equip.instance)
                        a3_equip.instance.OpenBSHC();
                    break;
                case 36:
                    if (a3_equip.instance)
                        a3_equip.instance.OpenBSXQ();
                    break;
                case 43:
                    if (a3_equip.instance)
                        a3_equip.instance.OpenBSXQ();
                    break;
                case 44:
                    if (a3_equip.instance)
                        a3_equip.instance.OpenBSXQ();
                    break;
                case 31://比武场
                    if (a3_sports._instantiate) {
                        a3_sports._instantiate.OpenPVP();
                    }
                    break;

                case 46://战场
                    if (a3_sports._instantiate)
                    {
                        a3_sports._instantiate.OpenJDZC ();
                    }
                    break;
                case 47: // 竞技入口
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.OpenSport", "ui/interfaces/low/a1_low_fightgame");
                    break;

                case 48:
                    //坐骑
                    if (a3_expbar.instance)
                        a3_expbar.instance.OpenMOUNT ();

                    if ( a1_gamejoy.inst_joystick )
                    a1_gamejoy.inst_joystick.OpenRide();
                    break;
                case 49:
                    InterfaceMgr.doCommandByLua("a1_low_fightgame.Openhuoyue", "ui/interfaces/low/a1_low_fightgame");
                    break;

               
            }
        }

        public int id;              //功能id
        public int type;            //1主线任务,2转生等级,3军团商店
        public int main_task_id;    //主线任务id
        public int zhuan;			//转生
        public int lv;				//等级
        public int legionlvl;       //军团等级

        public bool show;           //动画部分
        public float pos_x;
        public float pos_y;

        public int func_id;//icon对应id；
        public int icon;
        public string name;
        public string des;
        public int grade;
        public int level;


        public bool opened = false;
        public bool Opened => checkOpen((int)PlayerModel.getInstance().up_lvl, (int)PlayerModel.getInstance().lvl, A3_TaskModel.getInstance().main_task_id);
        public bool checkOpen(int zhuanshen, int lvl, int maintaskid, bool finished = false, bool showAni = false)
        {
            if (opened)
                return true;
            if (type == 2)
            {
                //if (zhuanshen  * 100 + lvl >= zhuan * 100 + lv) {
                //	doOpen(showAni);
                //	opened = true;
                //	return true;
                //}
                if (zhuanshen > zhuan)
                {
                    doOpen(showAni);
                    opened = true;
                    return true;
                }
                else if (zhuanshen == zhuan)
                {
                    if (lvl >= lv)
                    {
                        doOpen(showAni);
                        opened = true;
                        return true;
                    }
                }
            }
            else if (type == 1)
            {
                if (maintaskid > main_task_id || (maintaskid == main_task_id && finished))
                {
                    doOpen(showAni);
                    opened = true;
                    return true;
                }
            }
            return false;
        }

        public bool legionopen => checklegion(A3_LegionModel.getInstance().myLegion.lvl);

        public bool checklegion(int legionlvls)
        {
            if (type == 3)
            {
                if (legionlvls >= legionlvl)
                {
                    return true;
                }
                else
                    return false;
            }

            return false;
        }

    }

    public class FunctionOpenMgr
    {
        public static int PK_MODEL = 1;//PK模式切换
        public static int DAILY_TASK = 2;//日常任务
        public static int AUCTION_GUILD = 3;//拍卖行
        public static int ACHIEVEMENT = 4;//成就
        public static int SUMMON_MONSTER = 5;//召唤兽
        public static int SKILL = 6;//技能
        public static int PET_SWING = 7;//翼灵
        public static int PET = 8;//灵宠
        public static int WING = 9;//飞翼
        public static int ACTIVITES = 10;//活动
        public static int WIND_THRONE = 11;//风神王座
        public static int CHASTEN_JAIL = 12;//魔炼之地
        public static int GOLD_DUNGEON = 13;//金币副本
        public static int EXP_DUNGEON = 14;//经验副本
        public static int SUMMON_PARK = 15;//召唤兽乐园
        public static int GLOBA_BOSS = 16;//世界BOSS
        public static int DEVIL_HUNTER = 17;//魔物猎人//圣器公用
        public static int PVP_DUNGEON = 31;//竞技场
        public static int SPORT_JDZC = 46;  // 据点战场
        public static int FOR_CHEST = 32;//抢宝箱
        public static int COUNTERPART = 33;//副本入口
        public static int TEAMPART = 34;//组队副本
        public static int LEGION = 39;//军团商店
        public static int ENTRUST_TASK = 37;//委托任务
        public static int AUTO_PLAY = 41;//"我要升级"面板上的自动挂机
        public static int EQP = 18;//装备
        public static int EQP_ENHANCEMENT = 19;//装备强化
        public static int EQP_REMOLD = 20;//装备重铸
        public static int EQP_INHERITANCE = 21;//装备传承
        public static int EQP_LVUP = 22;//装备进阶
        public static int EQP_MOUNTING = 23;//装备宝石
        public static int EQP_ENCHANT = 24;//装备追加
        public static int EQP_BSHC = 35;//宝石合成
        public static int EQP_BSXQ = 36;//宝石镶嵌
        public static int SCREAMINGBOX = 25;//魔盒
        public static int HUDUN = 30;//护盾
        public static int STAR_PIC=42;//星图
        public static int RUNE = 43;//符文
        public static int JL_SL = 44;//精灵试炼
        public static int SEVEN_DAY = 45;//7日登录

        public static int SPORT = 47;//竞技入口

        public static int MOUNT = 48;//坐骑

        public static int HUOYUE = 49;//活跃入口

        //public static int VIP_EXPERIENCE = 26;//体验VIP开通
        //public static int DRESS = 27;//时装
        //public static int GEM = 28;//宝石
        //public static int FAMILY = 29;//家族

        //public static int BI_WU = 31;//比武场
        //public static int GUAJI = 32;//挂机场景
        //public static int EQUIP_CANVANCE = 33; //装备合成

        static FunctionOpenMgr _instance;
        public static FunctionOpenMgr instance
        {
            get
            {
                if (_instance == null) init();
                return _instance;
            }
        }
        public static void init()
        {
            if (_instance != null) return;
            _instance = new FunctionOpenMgr();
            _instance.initMgr();
        }

        public Dictionary<int, FunctionItem> dItem = new Dictionary<int, FunctionItem>();
        public void initMgr()
        {
            int lv = (int)PlayerModel.getInstance().lvl;
            var xml = XMLMgr.instance.GetSXML("func_open");
            var list = xml.GetNodeList("func");
            foreach (var v in list)
            {
                FunctionItem item = new FunctionItem();
                item.id = v.getInt("id");
                item.type = v.getInt("type");
                item.show = v.getInt("show") == 1 ? true : false;
                item.pos_x = v.getFloat("state_x");
                item.pos_y = v.getFloat("state_y");
                if (item.type == 1)
                {
                    int param = v.getInt("param1");
                    item.main_task_id = param;
                }
                else if (item.type == 2)
                {
                    string[] param = v.getString("param1").Split(',');
                    item.zhuan = int.Parse(param[0]);
                    item.lv = int.Parse(param[1]);
                }
                else if (item.type == 3)
                {
                    item.legionlvl = v.getInt("param1");
                }
                dItem[item.id] = item;
            }
        }






        public void onLvUp(int up_lvl, int lvl, bool showAni = false)
        {

            foreach (FunctionItem item in dItem.Values)
            {
                item.checkOpen(up_lvl, lvl, 0, false, showAni);
            }
        }

        public void onFinshedMainTask(int maintaskid, bool finished = false, bool showAni = false)
        {

            foreach (FunctionItem item in dItem.Values)
            {
                item.checkOpen(0, 0, maintaskid, finished, showAni);
            }
        }

        public int getLvNeed(int id)
        {
            if (!dItem.ContainsKey(id))
                return -1;
            return dItem[id].lv;
        }

        public bool Check(int id, bool notice = false)
        {
            if (!dItem.ContainsKey(id))
                return false;
            if (!dItem[id].Opened && notice)
                flytxt.instance.fly(ContMgr.getCont("gameroom_unopen"));
            return dItem[id].opened;
        }

        public bool checkLv(int id, bool notice = false)
        {
            if (!dItem.ContainsKey(id))
                return false;
            if (dItem[id].Opened && notice)
                flytxt.instance.fly(ContMgr.getCont("gameroom_unopen"));
            return dItem[id].opened;
        }
        public bool checkLegion(int id, bool notice = false)
        {
            if (!dItem.ContainsKey(id))
                return false;
            if (dItem[id].legionopen && notice)
                flytxt.instance.fly(ContMgr.getCont("gameroom_unopen"));
            return dItem[id].legionopen;
        }

        public void tryOpenFunction(int id)
        {
            if (checkLv(id))
                dItem[id].doOpen();
        }

    }



}
