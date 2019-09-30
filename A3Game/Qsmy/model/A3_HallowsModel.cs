using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MuGame
{
    class A3_HallowsModel : ModelBase<A3_HallowsModel>
    {

        public static int type_duihuan = 0;
        public int soul_num;
        public Dictionary<int, hallowsData> now_hallows_dic=new Dictionary<int, hallowsData>();
        public   List<Dictionary<int,float>> attributes;
        public A3_HallowsModel()
        {
            attributes = new  List<Dictionary<int, float>>();
            for(int i =1;i<10;i++)
            {
                hallowsData hd = new hallowsData();
                hd.id = i;
                hd.item_id = 0;
                hd.lvl = 1;
                hd.exp = 0;
                now_hallows_dic[hd.id] = hd;
            }
        }

        //圣魂数量
        public string GetSoulNum()
        {
            return soul_num.ToString();
        }
        //身上装备的东西
        public Dictionary<int, hallowsData> now_hallows()
        {         
            return now_hallows_dic.Count>0? now_hallows_dic:null;
        }
        //不同品质的属性
        public List<Dictionary<int,float>> GetAttributeForQuality(int quality)
        {
            attributes.Clear();
           List<SXML> xml= XMLMgr.instance.GetSXML("holicware.basic_att", "lv==" + quality).GetNodeList("att");
            for(int i=0;i<xml.Count;i++)
            {
                Dictionary<int, float> dic = new Dictionary<int, float>();
                dic[xml[i].getInt("att_type")] = xml[i].getFloat("att_value");
                attributes.Add(dic);

            }
           
            if (attributes.Count <= 0)
                return null;
            else
                return attributes;
        }
        //位置和item_id寻找其他属性
        public hallows_skill_data GetHallowsSkillData(int id,int item_id)
        {
            hallows_skill_data data = new hallows_skill_data();
            SXML  xml= XMLMgr.instance.GetSXML("holicware.holic", "id==" + id).GetNode("att", "item==" + item_id);
            data.quality = xml.getInt("quality");
            data.skill_name = xml.getString("name");
            data.skill_des = xml.getString("des");
            data.skill_id = xml.getInt("icon");

            return data;
        }
        //同过item_id找位置
        public int GetTypeByItemid(int item_id)
        {
            return XMLMgr.instance.GetSXML("item.item", "id==" + item_id).getInt("holic_part");
        }
    }
    public  class hallowsData
    {
        public int id;            //位置
        public int lvl;
        public int exp;
        public int item_id;      
        public hallows_skill_data h_s_d;
        
    }
    public class hallows_skill_data
    {
        public int quality;
        public string skill_name;
        public string  skill_des;
        public int skill_id;
         
    }

}
