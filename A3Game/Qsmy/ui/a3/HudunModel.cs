using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using MuGame;
using System.Collections;
using UnityEngine;

namespace MuGame
{
    class HudunModel : ModelBase<HudunModel>
    {
        a3_hudun hudun = a3_hudun._instance;
        a3_BagModel bagModel = a3_BagModel.getInstance();
        public uint auto_time = 60; //自动充能时间
        public uint noAttackTime = 60;//脱战时间
        public bool isNoAttack = true; // 是否没有受到攻击，true为没有受到，false为受到攻击。当受到攻击调用a3_herohead.wait_attack()来计时脱战时间
        public bool is_auto = true;
        public Dictionary<int, hudunData> hdData = new Dictionary<int, hudunData>();
        public HudunModel()
            : base()
        {
            Readxml();
        }
        
        //当前护盾值
        private int nowCount = 0;
        public Action OnnowCountChange = null;
        public int NowCount
        {
            get { return nowCount; }
            set
            {
                if (nowCount == value)
                    return;

                nowCount = value;
                if (OnnowCountChange != null)
                    OnnowCountChange();
                if (a3_herohead.instance) 
                {
                    //a3_herohead.instance.refreshSheild();
                }
            }
        }
        //护盾等级
        private int level = 0;
        public Action OnLevelChange = null;
        public int Level
        {
            get { return level; }
            set
            {
                if (level == value)
                    return;

                level = value;

                if (OnLevelChange != null)
                    OnLevelChange();
                if (a3_herohead.instance)
                {
                    //a3_herohead.instance.show_sheild();
                }
            }
        }
        //自动充能
        //public void Add_energy_auto()
        //{
        //    if (is_auto)
        //    {
        //        if (level <= 0) { flytxt.instance.fly("神圣护盾等级为零！！", 1); }
        //        else
        //        {
        //            if (nowCount >= GetMaxCount(level))
        //            {
        //                flytxt.instance.fly("神圣护盾的能量已满！！", 1);
        //            }
        //            else
        //            {
        //                if (OnMjCountOk_auto(hdData[level].addcount))
        //                {
        //                    hudunProxy.sendinfo(2);
        //                }
        //                else
        //                {
        //                    flytxt.instance.fly("魔晶数量不足！！", 1);
        //                }
        //            }
        //        }
        //    }
        //}
        public bool OnMjCountOk_auto(int needcount)
        {
            return needcount <= bagModel.getItemNumByTpid(1540);
        }

        public bool CheckLevelupAvailable()
        {
            if (Level < hudunData.max_level) 
            {
                if (GetNeedMjMun(Level + 1) <= a3_BagModel.getInstance().getItemNumByTpid(1540)
                        && GetNeedZhuan(Level + 1) <= PlayerModel.getInstance().up_lvl
                            && GetNeedLevel(Level + 1) <= PlayerModel.getInstance().lvl)
                    return true;
            }
            if (Level > 0 && NowCount < GetMaxCount(Level))
                if (isNoAttack)
                        if (GetNeedAddCount(Level) <= a3_BagModel.getInstance().getItemNumByTpid(1540))
                            return true;
            return false;
        }

        void Readxml()
        {
            List<SXML> xml = XMLMgr.instance.GetSXMLList("pvpsheild.sheild");
            hudunData.max_level = xml.Count;
            foreach (SXML x in xml)
            {
                hudunData info = new hudunData();
                info.hdLvl = x.getInt("lv");
                info.raise_zhuan = x.getInt("raise_zhuan");
                info.raise_lv = x.getInt("raise_lv");
                info.needcount = x.getInt("item_cost");
                info.addcount = x.getInt("item_refill");
                info.energy = x.getInt("energy");
                hdData[info.hdLvl] = info;
            }
        }
        ////获得当前等级最大护盾值

        public int GetMaxCount(int hdlvl)
        {
            return hdData[hdlvl].energy;
        }
        //获得强化所需魔晶数量
        public int GetNeedMjMun(int hdlvl) => hdData[hdlvl].needcount;

        //获得充能所需物品数量
        public int GetNeedAddCount(int hdlvl) => hdData[hdlvl].addcount;

        //获得强化所需的等级和转生
        public int GetNeedLevel(int hdlvl) => hdData[hdlvl].raise_lv;
        public int GetNeedZhuan(int hdlvl) => hdData[hdlvl].raise_zhuan;
    }
    class hudunData
    {
        public static int max_level;
        public int raise_zhuan; //升级需要的转生数
        public int raise_lv; //升级需要的等级
        public int hdLvl; // 护盾等级
        public int needcount; //升级消耗的数量
        public int addcount;//充能消耗的物品数量
        public int energy;//护盾的能量上限
    }
}
