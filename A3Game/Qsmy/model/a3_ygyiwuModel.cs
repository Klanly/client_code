using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using System.Collections;
using MuGame.Qsmy.model;
using UnityEngine;

namespace MuGame
{
    class a3_ygyiwuModel : ModelBase<a3_ygyiwuModel>
    {
        public Dictionary<int, yiwuInfo> Allywlist_God = new Dictionary<int, yiwuInfo>();
        public Dictionary<int, yiwuInfo> Allywlist_Pre = new Dictionary<int, yiwuInfo>();
        public Dictionary<int, yiwuInfo> yiwuList_God = new Dictionary<int, yiwuInfo>();
        public Dictionary<int, yiwuInfo> yiwuList_Pre = new Dictionary<int, yiwuInfo>();
        public int nowGod_id = -1;
        public int nowPre_id =0;
        public int nowGodFB_id=0;
        public int nowPreFB_id=0;
        public int nowPre_needupLvl = 0;
        public int nowPre_needLvl = 0;
        public uint yiwuLvl = 0;//研究遗物等级
        public uint studyTime = 0;//研究剩余时间
        //public uint studyTime_all = 0;//研究下一等级总时间
        public a3_ygyiwuModel() : base()
        {
            SXML xml = XMLMgr.instance.GetSXML("accent_relic.relic", "carr==" + PlayerModel.getInstance().profession);
            List<SXML> info = xml.GetNodeList("relic_god");
            for (int i =0;i<info.Count;i++)
            {
                yiwuInfo yiwuItem = new yiwuInfo();
                yiwuItem.id = info[i].getInt("id");
                yiwuItem.need_zdl = info[i].getInt("zdl");
                yiwuItem.isGod = true;
                yiwuItem.name = info[i].getString("name");
                yiwuItem.des = info[i].getString("des");
                yiwuItem.needexp = info[i].getInt("exp");
                yiwuItem.place = info[i].getString("place");

                yiwuItem.awardName = info[i].getString("des1");
                yiwuItem.awardDesc = info[i].getString("des2");
                yiwuItem.iconid = info[i].getInt("icon");
                yiwuItem.awardId = info[i].GetNode("award").getInt("id");
                yiwuItem.awardType = info[i].GetNode("award").getInt("type");
                yiwuItem.eff = info[i].getString("eff");
                yiwuItem.fbBox_title = info[i].getString("title");
                yiwuItem.fbBox_dec = info[i].getString("desc");
                Allywlist_God[yiwuItem.id] = yiwuItem;
            }
            List<SXML> Pre_xml_list = XMLMgr.instance.GetSXMLList("accent_relic.relic", "type==" + 2);
            SXML Pre_xml = null;
            foreach ( SXML l in Pre_xml_list)
            {
                if (l.getInt ("carr") == PlayerModel.getInstance().profession)
                {
                    Pre_xml = l;
                }
            }
            List<SXML> info_pre = Pre_xml.GetNodeList("relic_god");
            for (int i = 0; i < info_pre.Count; i++)
            {
                yiwuInfo yiwuItem = new yiwuInfo();
                yiwuItem.id = info_pre[i].getInt("id");
                yiwuItem.need_zdl = info_pre[i].getInt("zdl");
                yiwuItem.isGod = false;
                yiwuItem.name = info_pre[i].getString("name");
                yiwuItem.des = info_pre[i].getString("des");
                yiwuItem.place = info_pre[i].getString("place");
                yiwuItem.awardName = info_pre[i].getString("des1"); 
                yiwuItem.awardDesc = info_pre[i].getString("des2");
                yiwuItem.iconid = info_pre[i].getInt("icon");
                yiwuItem.awardId = info_pre[i].GetNode("award").getInt("id");
                yiwuItem.awardType = info_pre[i].GetNode("award").getInt("type");
                yiwuItem.fbBox_title = info_pre[i].getString("title");
                yiwuItem.fbBox_dec = info_pre[i].getString("desc");
                yiwuItem.eff = info_pre[i].getString("eff");
                yiwuItem.needuplvl = info_pre[i].getInt("zhuan");
                yiwuItem.needlvl = info_pre[i].getInt("level");

                Allywlist_Pre[yiwuItem.id] = yiwuItem;
            }
        }

        public int GetZTime( int lvl)
        {
            SXML Pre_xml = XMLMgr.instance.GetSXML("accent_relic.relic_knowledge");
            int ZTime = Pre_xml.GetNode("level", "lvl==" + lvl).getInt("cost_time");
            return ZTime;
        }
        public yiwuInfo GetYiWu_God ( int id )
        {
            return Allywlist_God[id];
        }

        public yiwuInfo GetYiWu_Pre(int id)
        {
            return Allywlist_Pre[id];
        }

        public void loadList()
        {
            foreach (int id in Allywlist_God.Keys)
            {
                if (id < nowGod_id)
                {
                    if (!yiwuList_God.ContainsKey(id))
                    {
                        yiwuList_God[id] = Allywlist_God[id];
                    }
                }
            }
            foreach (int id in Allywlist_Pre.Keys)
            {
                if (id < nowPre_id)
                {
                    if (!yiwuList_Pre.ContainsKey (id))
                    {
                        yiwuList_Pre[id] = Allywlist_Pre[id];
                    }
                }
            }
        }
        public  bool canToNowPre()
        {
            if (!Allywlist_Pre.ContainsKey (nowPre_id)) { return false; }

            if (PlayerModel.getInstance().up_lvl < Allywlist_Pre[nowPre_id].needuplvl)
            {
                return false;
            }
            else if (PlayerModel.getInstance().up_lvl ==Allywlist_Pre[nowPre_id].needuplvl)
            {
                if (PlayerModel.getInstance().lvl < Allywlist_Pre[nowPre_id].needlvl)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (PlayerModel.getInstance().up_lvl > Allywlist_Pre[nowPre_id].needuplvl)
            {
                return true;
            }
            return false;
        }
    }

    class yiwuInfo
    {
        public int id;
        public bool isGod;//是否神王副本
        public int awardType;//奖励类型
        public int awardId;//奖励id
        public string name;//副本名称（技能符文 名）
        public string des;//副本背景介绍
        public int needexp;//挑战副本所需经验
        public string place;//副本挑战地点
        public string awardName;//副本奖励（技能符文）名称
        public string awardDesc;//副本奖励（技能符文）介绍
        public string eff;//（技能符文）效果
        public int iconid;//副本奖励（技能符文）图标
        public string fbBox_title;//挑战副本框标题
        public string fbBox_dec;//挑战副本框介绍
        public int needuplvl;//开启转级
        public int needlvl;//开启等级
        public int need_zdl;//推荐战斗力
    }
}
