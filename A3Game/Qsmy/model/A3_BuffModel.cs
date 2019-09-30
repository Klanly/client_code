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
    public enum BUFF_TYPE
    {
        NULL = 0,
        CANT_MOVE = 1, 
        CANT_SKILL = 6,
        CANT_MOVE_SKILL = 7
    }
    class A3_BuffModel : ModelBase<A3_BuffModel>
    {
        //public Dictionary<uint, BuffInfo> nowBuffList = new Dictionary<uint, BuffInfo>();//用来显示buff状态
        //public Dictionary<uint, int> BuffImg = new Dictionary<uint, int>();
        //public Dictionary<uint, string> BuffText = new Dictionary<uint, string>();


        public Dictionary<uint, BuffInfo> BuffCd = new Dictionary<uint, BuffInfo>();

        public List<BUFF_TYPE> Buff_type_list = new List<BUFF_TYPE>();
        public void addBuffList(Variant data)
        {
            BuffInfo buff = new BuffInfo ();
            buff.id = data["id"];
            buff.par = data["par"];
            buff.start_time =data["start_tm"];
            buff.end_time =data["end_tm"];
            #region
            SXML xml = XMLMgr.instance?.GetSXML("skill.state", "id==" + buff.id);
            SXML xml_node = xml?.GetNode("s", null);
            if (xml_node != null)
            {
                switch (xml_node.getInt("tp"))
                {
                    case 1:
                        buff.buff_type = BUFF_TYPE.CANT_MOVE;
                        SelfRole._inst.can_buff_move = false;
                        break;
                    case 6:
                        SelfRole._inst.can_buff_skill = false;
                        buff.buff_type = BUFF_TYPE.CANT_SKILL;
                        break;
                    case 7:
                        SelfRole._inst.can_buff_move = false;
                        SelfRole._inst.can_buff_skill = false;
                        SelfRole._inst.can_buff_ani = false;
                        SelfRole._inst.m_curAni.enabled = false;
                        buff.buff_type = BUFF_TYPE.CANT_MOVE_SKILL;
                        break;
                    default:
                        buff.buff_type = BUFF_TYPE.NULL;
                        break;
                }

                if (buff.buff_type != BUFF_TYPE.NULL && !Buff_type_list.Contains(buff.buff_type))
                {
                    Buff_type_list.Add(buff.buff_type);
                }
            }

            buff.icon = xml?.getString("icon");
            buff.name = xml?.getString("name");
            #endregion
                                   
            
            dele_buff(data);
         
            BuffCd[buff.id] = buff;
            
            buff.doCD();
           
            //if (buff.id >= 300 && buff.id < 2952)
            //    a3_buff.instance.skillList.Add(buff.id);
            a3_buff.instance?.resh_buff();
        }
        public void dele_buff(Variant data)//组队buff中服务器只发添加没有发删除buff
        {
            for (uint i = 9998; i > 9995; i--)
            {
                if (data["id"] > i)
                {
                    if (BuffCd.ContainsKey(i))
                        BuffCd.Remove(i);
                }
            }

            //if (BuffCd.ContainsKey(data["id"]))
            //{

            //    BuffCd.Remove(data["id"]);

            //}
        }

        public void RemoveBuff(uint id)
        {
            if (!BuffCd.ContainsKey(id))
                return;

            BUFF_TYPE type = BuffCd[id].buff_type;

            switch (type)
            {
                case BUFF_TYPE.CANT_MOVE:
                    SelfRole._inst.can_buff_move = true;
                    break;
                case BUFF_TYPE.CANT_SKILL:
                    SelfRole._inst.can_buff_skill = true;
                    break;
                case BUFF_TYPE.CANT_MOVE_SKILL:
                    SelfRole._inst.can_buff_move = true;
                    SelfRole._inst.can_buff_skill = true;
                    SelfRole._inst.can_buff_ani = true;
                    SelfRole._inst.m_curAni.enabled = true;
                    break;
            }

            if (SelfRole._inst is P3Mage)
            {
                //法师盾的buffid
                SXML xml = XMLMgr.instance?.GetSXML("skill.state", "id==" + id);
                if (xml.getInt("skill_id") == 3008)
                    SelfRole._inst.PlaySkill(30081);
            }
          

            if (type != BUFF_TYPE.NULL && Buff_type_list.Contains(type))
                Buff_type_list.Remove(BuffCd[id].buff_type);

          
            BuffCd.Remove(id);

            a3_buff.instance?.resh_buff();
        }


        public void addOtherBuff(BaseRole role, uint id)
        {
            SXML xml = XMLMgr.instance.GetSXML("skill.state", "id==" + id);
            if(xml!=null)
            {
                SXML xml_node = xml.GetNode("s", null);
                if (xml_node != null)
                {
                    switch (xml_node.getInt("tp"))
                    {
                        case 7:
                            role.m_curAni.enabled = false;
                            break;
                    }
                }
            }
           
        }
        public void removeOtherBuff(BaseRole role, uint id)
        {
            SXML xml = XMLMgr.instance.GetSXML("skill.state", "id==" + id);
            if (xml != null)
            {
                SXML xml_node = xml.GetNode("s", null);
                if (xml_node != null)
                {
                    switch (xml_node.getInt("tp"))
                    {
                        case 7:
                            role.m_curAni.enabled = true;
                            break;
                    }
                }
            }

            if (role is P3Mage || role is ohterP3Mage)
            {
                //法师盾的buffid
                if (xml.getInt("skill_id") == 3008)
                    SelfRole._inst.PlaySkill(30081);
            }
        }
    }
    class BuffInfo
    {
        public uint id;
        public uint par;
        public uint start_time;
        public uint end_time;
        public bool isfristShow = true;
        public long endCD = 0;
        public string icon;
        public string name;
        public BUFF_TYPE buff_type = BUFF_TYPE.NULL;
        public int cdTime
        {
            get
            {
                long t = muNetCleint.instance.CurServerTimeStampMS;
                if (endCD < t)
                {
                    endCD = 0;
                    return 0;
                }

                return (int)(endCD - t);
            }
        }
        public void doCD()
        {
            long tempCd = muNetCleint.instance.CurServerTimeStampMS + (end_time - start_time);
            if (endCD < tempCd)
                endCD = tempCd;
        }

        public void update(long timestp)
        {
            if (endCD == 0)
                return;
            if (endCD < timestp)
            {
                endCD = 0;
            }
        }
    }
}
