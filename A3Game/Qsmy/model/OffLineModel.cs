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
    class OffLineModel : ModelBase<OffLineModel>
    {
        private List<List<float>> rateList = new List<List<float>>();

        //最大累计时间上线为1天
        public static int maxTime = 86400;
        //最小累计时间为1小时
        public static int minTime = 3600;

        public OffLineModel()
            : base()
        {

        }

        public bool ismaxlvl = false;
        //基础离线时间
        private int offLineTime = 0;
        public Action OnOffLineTimeChange = null;
        public int OffLineTime
        {
            get { return offLineTime; }
            set
            {
                if (value > maxTime)
                    value = maxTime;


                offLineTime = value;

                if (OnOffLineTimeChange != null)
                    OnOffLineTimeChange();
            }
        }

        //基础离线经验
        private int baseExp = 0;
        public Action OnBaseExpChange = null;
        public int BaseExp
        {
            get { return baseExp; }
            set
            {
                baseExp = value;

                if (OnBaseExpChange != null)
                    OnBaseExpChange();
                if (a3_expbar.instance != null)
                    a3_expbar.instance.btnAutoOffLineExp.gameObject.SetActive(CanGetExp);
            }
        }

        //是否可以领取离线经验
        private bool canGetExp = false;
        public bool IsHaveEQP= false;
        public bool CanGetExp
        {
            get 
            {
                if (this.BaseExp > 0)
                    canGetExp = true;
                else
                {
                    if (PlayerModel.getInstance().up_lvl >= 10 && PlayerModel.getInstance().lvl >= 165)
                        canGetExp = IsHaveEQP;
                    else
                        canGetExp = false;
                }
 
                return canGetExp; 
            }
        }

        //离线经验XML表
        private SXML offLineXML;
        public SXML OffLineXML
        {
            get 
            {
                if (offLineXML == null)
                    offLineXML = XMLMgr.instance.GetSXML("offlineExp_a3");

                return offLineXML;
            }
        }

        //获得双倍离线经验
        public int GetDoubleExp()
        {
            return BaseExp * 2;
        }

        //获得三倍经验
        public int GetThreefoldExp()
        {
            return BaseExp * 3;
        }

        //获得4倍经验
        public int GetQuadrupleExp()
        {
            return BaseExp * 4;
        }

        //每十分钟的花费
        public float GetCost(int type)
        {
            float cost = 0;
            float cost_value = GetRateByType(type);

            int rate = (int)Math.Floor((float)OffLineTime / 600);
            cost = cost_value * rate;

            return (float)Math.Ceiling(cost);
        }

        //根据经验倍数获得花费倍数
        public float GetRateByType(int type)
        {
            SXML _xml = OffLineXML.GetNode("rate_type", "type==" + type.ToString());
            float rate = _xml.getFloat("cost_value");

            return rate;
        }

        //更具经验倍数获得花费类型, 0 无花费,1 金币, 2 钻石
        public int GetSpendTypeByType(int type)
        {
            SXML _xml = OffLineXML.GetNode("rate_type", "type==" + type.ToString());
            int _type = _xml.getInt("cost_type");

            return _type;
        }
            
    }

}
