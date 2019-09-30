using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;

using GameFramework;
namespace MuGame
{
    public class ClientOutGameConf : configParser
    {
        public ClientOutGameConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientOutGameConf( m as ClientConfig );
        }
        protected override Variant _formatConfig(Variant conf)
        {
            if(conf.ContainsKey("createPro"))
            {
                conf["createPro"] = GameTools.array2Map(conf["createPro"],"tp");
            }
            
            if(conf.ContainsKey("ranName"))
            {
                conf["ranName"] = GameTools.array2Map(conf["ranName"],"tp");
            }
            
            // TO DO : add more format segment	
            return conf;
        }
                //创建界面属性值配置
        public Variant get_createPro_conf()
        {
            if(this.m_conf == null)
            {
                return null;	
            }
            return this.m_conf["createPro"];
        }
        
        //随机名称配置
        public Variant get_ranName_conf()
        {
            if(this.m_conf == null)
            {
                return null;	
            }
            return this.m_conf["ranName"];
        }
        
        //获取随机名称的方式
        public int gerRanNameType()
        {
            if(this.m_conf == null)
            {
                return 0;	
            }
            return m_conf["ranName"][0]["ranType"][0]["type"];
        }
        
        //获取姓氏数组
        public Variant getFirstNameArr()
        {
            if(this.m_conf == null)
            {
                return null;	
            }
            string nameStr = m_conf["ranName"]["0"]["firstName"][0]["value"];
            Variant nameArr = GameTools.split(nameStr, ",");
            return nameArr;
        }
        
        //根据类型获取名字数组
        public Variant getLastNameArr(int carr,int sex)
        {
            if(this.m_conf == null)
            {
                return null;	
            }
            Variant lastNameArr = m_conf["ranName"]["0"]["lastName"] as Variant;
            Variant nameData = new Variant();
            foreach(Variant data in lastNameArr._arr)
            {
                if(carr == data["carr"] && sex == data["sex"])
                {
                    nameData = data;
                    break;
                }
            }
            if(nameData == null)
            {
                //保护一下(万一没配取第一个)
                nameData = lastNameArr[0];
            }
            string nameStr = nameData["value"];
            Variant nameArr = GameTools.split(nameStr, ",");
            return nameArr;
        }
    }
}
