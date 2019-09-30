using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using System.Text.RegularExpressions;

using GameFramework;
namespace MuGame
{
    public class ClientShieldConf : configParser
    {
        public ClientShieldConf( ClientConfig m ):base(m)
        {
            
        }
        public static IObjectPlugin create(IClientBase m)
        {           
            return new ClientShieldConf( m as ClientConfig );
        }
        protected override Variant _formatConfig(Variant conf)
        {
            // TO DO : add more format segment
            return conf;
        }
                
        private Variant _strArr = null; //文字数据数组
        
        private void _formatStr()
        {
            _strArr = GameTools.split(m_conf["shield"][0]["text"], ",");
            
            //清掉文本中的空格
            
            //Regex myPattern = new Regex(@"\n\s*\r");// /\s+/g;

            for(int i = 0; i < _strArr.Length; i++){				
                _strArr[i] = Regex.Replace(_strArr[i]._str,@"\s","");
            }
        }
        
        /**
         *查看字符中是否有相同的字符
         * @str 输入的字符
         * arr[0]是否有 false 没有 true 有
         * arr[1]如果有返回和谐后的新句子
         */
        public Variant isHaveWord(string str)
        {
            if(_strArr == null)
            {
                _formatStr();
            }
            
            //正则替换空格		
            //var myPattern:RegExp = /\s+/g;
            string  c_str = Regex.Replace(str,@"\s","");
            
            Variant arr = new Variant();
            Boolean ishave = false;
            for(int i = 0; i < _strArr.Length; i++){
                string s = _strArr[i];	
                
                if(s == "")
                    continue;
                
                while(true){						
                    if(c_str.IndexOf(s) != -1){	
                        c_str = c_str.Replace(s,"*");					
                        ishave = true;
                    }
                    else{
                        break;
                    }	
                }
            }
            arr._arr.Add(ishave);
            arr._arr.Add(c_str);
            return arr;
        }
    }
}
