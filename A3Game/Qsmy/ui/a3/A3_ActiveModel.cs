using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cross;

namespace MuGame
{
    class A3_ActiveModel : ModelBase<A3_ActiveModel>
    {
        public int blessnum_yb = 0;   //元宝的鼓舞次数
        public int blessnum_ybbd = 0; //绑定元宝的鼓舞次数

        //魔物猎人副本
        public int mwlr_charges = 0;        //已经完成的次数
        public int vip_buy_count = 0;       //vip已购买次数
        public Variant mwlr_map_info = new Variant(); //狩猎目标        
        public int mwlr_doubletime = 0; //双倍奖励剩余时间
        public int mwlr_totaltime = 0; //总猎杀时间
        public List<int> mwlr_map_id = new List<int>();     //狩猎目标所在地图
        public Dictionary<int,Vector3> mwlr_mons_pos = new Dictionary<int, Vector3>();
        public bool mwlr_giveup;
        public Vector3 mwlr_target_pos { get; set; } = Vector3.zero;
        public List<int> listKilled;
        public bool _mwlr_on;
        public bool mwlr_on {
            get
            {
                return _mwlr_on;
            }
            set
            {
                _mwlr_on = value;
                if (_mwlr_on)
                {
                    if (mwlr_target_monId < 0)
                    {
                        _mwlr_on = false;
                        return;
                    }
                    if (!PlayerModel.getInstance().task_monsterIdOnAttack.ContainsKey(-1))
                        PlayerModel.getInstance().task_monsterIdOnAttack.Add(-1, mwlr_target_monId);
                    else
                        PlayerModel.getInstance().task_monsterIdOnAttack[-1] = mwlr_target_monId;
                }
                else if (PlayerModel.getInstance().task_monsterIdOnAttack.ContainsValue(mwlr_target_monId))
                {
                    mwlr_target_pos = Vector3.zero;
                    PlayerModel.getInstance().task_monsterIdOnAttack.Remove(-1);
                    mwlr_target_monId = -1;
                }

            }
        }
        public int mwlr_target_monId { get; set; }
        public int mwlr_mon_killed { get; set; }
        public A3_ActiveModel() : base()
        {
            //int num;
           // if (A3_VipModel.getInstance().Level > 0)
            //    num = A3_VipModel.getInstance().vip_exchange_num(7);
            //else
            //    num = 10;

            SXML Xml = XMLMgr.instance.GetSXML("jjc.info");
            if (Xml == null) return;

            buy_cnt = A3_VipModel.getInstance().vip_exchange_num(7);

            callenge_cnt = Xml.getInt("callenge_cnt");

             buy_zuan_count = Xml.getInt("buy_cost");
            listKilled = new List<int>();
        }
        //试炼之地
        public int maxlvl = 0;
        public int nowlvl = 0;
        public int count_mlzd = 0;
        public long Time = 0;
        public int sweep_type= 0;

        //竞技场
        public int pvpCount = 0; //匹配次数
        public int buyCount = 0; //购买次数
        public int callenge_cnt = 0;
        public int buy_cnt = 0;
        public int score = 0; //分数
        public int grade = 0; //段位
        public int lastgrage = 0;//上届段位
        public int buy_zuan_count = 0;
        public int Canget = 0;


    }
}
