using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MuGame
{
    public class ResetLvLModel : ModelBase<ResetLvLModel>
    {
        public ResetLvLModel()
            : base()
        {


        }
        /// 根据玩家职业,已转次数,等级查找玩家可达到的最大经验值
        public uint getExpByResetLvL(int profession, uint zhuan, uint lvl)
        {
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);
            SXML xmlLvl = xmlZhuan.GetNode("carr", "lvl==" + lvl);
            return xmlLvl == null ? 0 : xmlLvl.getUint("exp");
        }
        // 根据已转生次数查找再次转生的需要等级
        public uint getNeedLvLByCurrentZhuan(int profession, uint zhuan)
        {

            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            if (xmlCarr == null) return uint.MaxValue;
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);
            List<SXML> xmlLvl = xmlZhuan.GetNodeList("carr", null);

            return xmlLvl[xmlLvl.Count - 1].getUint("lvl");
        }

        // 根据已转生次数获得再次转生需要的经验大小
        public uint getNeedExpByCurrentZhuan(int profession, uint zhuan)
        {
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            if (xmlCarr == null) return uint.MaxValue;
            
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);
            List<SXML> xmlLvl = xmlZhuan.GetNodeList("carr", null);

            return xmlLvl[xmlLvl.Count - 1].getUint("exp");
        }
        //根据转生次数获取奖励属性点
        public uint getAwardAttrPointByZhuan(int profession, uint zhuan)
        {
            zhuan++;
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);

            return xmlZhuan == null ? 0 : xmlZhuan.getUint("att_pt");
        }
        //根据转生次数获取下一次转生等级
        public uint getNextLvLByZhuan(int profession, uint zhuan,uint exp)
        {
            uint lvl = 0;
            zhuan++;
            uint expTemp=0;
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);
            List<SXML> xmlNodeCarrArry = xmlZhuan.GetNodeList("carr");
            for (int i = 0; i < xmlNodeCarrArry.Count; i++)
            {

                expTemp += xmlNodeCarrArry[i].getUint("exp");
                if (exp < expTemp)
                {
                    //if (i > 0)
                    //{
                        lvl = xmlNodeCarrArry[i].getUint("lvl");
                        break;
                    //}
                }
       
            }
            return lvl;
        }
        //根据转生次数获取奖励需要的金币
        public uint getNeedGoldsByZhuan(int profession, uint zhuan)
        {
            //zhuan++;
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);

            return xmlZhuan == null ? 0 : xmlZhuan.getUint("money_cost");
        }
        //根据玩家职业,转生次数获得可达到的总经验值
        public uint getAllExpByZhuan(int profession, uint zhuan)
        {
            zhuan++;
            uint allExp = uint.MinValue;
            SXML xmlCarr = XMLMgr.instance.GetSXML("carrlvl.carr", "carr==" + profession);
            SXML xmlZhuan = xmlCarr.GetNode("zhuanshen", "zhuan==" + zhuan);
            uint lvl = xmlZhuan.getUint("exp_pool_level");
            lvl--;
            List<SXML> xmlCarrLvLList = xmlZhuan.GetNodeList("carr", null);
            for (int i = 0; i < xmlCarrLvLList.Count; i++)
            {
                if (xmlCarrLvLList[i].getUint("lvl") <= lvl) allExp += xmlCarrLvLList[i].getUint("exp");
            }
            return allExp;
        }
    }
}
