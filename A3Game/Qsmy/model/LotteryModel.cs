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

    class LotteryModel : ModelBase<LotteryModel>
    {
        public Dictionary<uint, lotterydata> dataybs = new Dictionary<uint, lotterydata>();
        public  List<itemLotteryAwardData> lotteryAwardItems = new List<itemLotteryAwardData>();
        public List<itemLotteryAwardInfoData> lotteryAwardInfoItems = new List<itemLotteryAwardInfoData>();
        public Vector3 boxPosition;
        public Vector3 boxRotation;
        public Vector3 boxScale;

        public Vector3 boxPosition1;
        public Vector3 boxRotation1;
        public Vector3 boxScale1;


        public float delayTime;
        public static int iceOnceCost = 50;
        public int lotteryTimeTenth,//十连抽的时间
            lotteryCostOnce,//单抽一次的价格
            lotteryCostTenth,//十连抽的价格
            lotteryCostVip;//高级抽价格

        public int needviplvl;
        public List<int> lotteryTimeOnce = new List<int>();//单抽一次的时间
        public bool isNewBie = false;

        public LotteryModel()
        {
            ReadLotteryXMLYB();
            ReadLotteryConfig();
        }
        public void ReadLotteryConfig()
        {
            SXML lotteryXML = XMLMgr.instance.GetSXML("lottery");
            List<SXML> lotteryOnceInfo = lotteryXML.GetNodeList("lottery_num_one");
            for (int i = 0; i < lotteryOnceInfo.Count; i++)
                lotteryTimeOnce.Add(lotteryOnceInfo[i].getInt("time"));
            lotteryTimeTenth = lotteryXML.GetNode("lottery_num_ten").getInt("time") ;
            lotteryCostOnce = lotteryXML.GetNode("lottery_money_one").getInt("cost");
            lotteryCostTenth = lotteryXML.GetNode("lottery_money_ten").getInt("cost");
            lotteryCostVip = lotteryXML.GetNode("vip_lottery").getInt("cost");

            needviplvl = lotteryXML.GetNode("vip_lottery").getInt("vip_lvl");

            float x, y, z, rotX, rotY, rotZ, scaleX, scaleY, scaleZ;
            float x1, y1, z1, rotX1, rotY1, rotZ1, scaleX1, scaleY1, scaleZ1;
            x = float.Parse(lotteryXML.GetNode("location").getString("x"));
            y = float.Parse(lotteryXML.GetNode("location").getString("y"));
            z = float.Parse(lotteryXML.GetNode("location").getString("z"));
            rotX = float.Parse(lotteryXML.GetNode("angle").getString("x"));
            rotY = float.Parse(lotteryXML.GetNode("angle").getString("y"));
            rotZ = float.Parse(lotteryXML.GetNode("angle").getString("z"));
            scaleX = float.Parse(lotteryXML.GetNode("scale").getString("x"));
            scaleY = float.Parse(lotteryXML.GetNode("scale").getString("y"));
            scaleZ = float.Parse(lotteryXML.GetNode("scale").getString("z"));

            x1 = float.Parse(lotteryXML.GetNode("location").getString("x1")); 
            y1 = float.Parse(lotteryXML.GetNode("location").getString("y1")); 
            z1 = float.Parse(lotteryXML.GetNode("location").getString("z1"));
            rotX1 = float.Parse(lotteryXML.GetNode("angle").getString("x1")); 
            rotY1 = float.Parse(lotteryXML.GetNode("angle").getString("y1"));
            rotZ1 = float.Parse(lotteryXML.GetNode("angle").getString("z1"));
            scaleX1 = float.Parse(lotteryXML.GetNode("scale").getString("x1")); 
            scaleY1 = float.Parse(lotteryXML.GetNode("scale").getString("y1")); 
            scaleZ1 = float.Parse(lotteryXML.GetNode("scale").getString("z1")); 

            boxPosition = new Vector3(x, y, z);
            boxRotation = new Vector3(rotX, rotY, rotZ);
            boxScale = new Vector3(scaleX, scaleY, scaleZ);
            boxPosition1 = new Vector3(x1, y1, z1);
            boxRotation1 = new Vector3(rotX1, rotY1, rotZ1);
            boxScale1 = new Vector3(scaleX1, scaleY1, scaleZ1);
            delayTime = lotteryXML.GetNode("delay_time").getFloat("val") * 0.1f;
        }
        public void ReadLotteryXMLYB()
        {
            lotteryAwardItems.Clear();
            List<SXML> xmls = XMLMgr.instance.GetSXMLList("lottery.item", null);
            for (int i = 0; i < xmls.Count; i++)
            {
                uint type = xmls[i].getUint("type");
                List<SXML> xmlItems = xmls[i].GetNodeList("item");
                lotterydata lotteryDataTemp = new lotterydata();
                lotteryDataTemp.type = type;
                if (dataybs.ContainsKey(type))
                {
                    dataybs[type] = lotteryDataTemp;
                }
                else
                {
                    dataybs.Add(type, lotteryDataTemp);
                }
                lotteryDataTemp.lotteryAwardItems = new List<itemLotteryAwardData>();
                for (int j = 0; j < xmlItems.Count; j++)
                {
                    itemLotteryAwardData lai = new itemLotteryAwardData();
                    lai.rootType = type;
                    lai.id = xmlItems[j].getUint("id");
                    lai.itemType = xmlItems[j].getUint("item_type");
                    lai.itemId = xmlItems[j].getUint("item_id");
                    lai.num = xmlItems[j].getUint("num");
                    lai.itemName = xmlItems[j].getString("item_name");
                    lai.cost = xmlItems[j].getUint("cost");
                    lai.stage = xmlItems[j].getUint("stage");
                    lai.intensify = xmlItems[j].getUint("intensify");
                    lotteryAwardItems.Add(lai);
                    lotteryDataTemp.lotteryAwardItems.Add(lai);
                }
            }
        }

        public string getAwardTypeId(uint id)
        {
            for (int i = 0; i < lotteryAwardItems.Count; i++)
            {
                if (id == lotteryAwardItems[i].id)
                {
                    if (lotteryAwardItems[i].itemType == 2)
                    {
                        //return "件";
                        return ContMgr.getCont("employer0");
                    }
                    if (lotteryAwardItems[i].itemType == 1)
                    {
                        //return "个";
                        return ContMgr.getCont("employer1");
                    }
                }
            }
            return string.Empty;
        }
        public int getAwardNumById(uint id)
        {
            for (int i = 0; i < lotteryAwardItems.Count; i++)
            {
                if (id == lotteryAwardItems[i].id)
                {
                    return (int)lotteryAwardItems[i].num;
                }
            }
            return -1;
        
        }
        public uint getAwardItemIdById(uint id)
        {
            for (int i = 0; i < lotteryAwardItems.Count; i++)
            {
                if (id == lotteryAwardItems[i].id)
                {
                    return lotteryAwardItems[i].itemId;
                  
                }
            }
            return 0;
        }
        public float getAwardItemAnimTimeSpan() => XMLMgr.instance.GetSXML("lottery.distance").getFloat("val");
    }
       


    public class lotterydata
    {
        public uint type;//奖品类型
        public List<itemLotteryAwardData> lotteryAwardItems;//奖品
    }
    public class itemLotteryAwardData//奖品
    {
        public uint rootType; 
        public uint id;
        public uint itemType;
        public uint itemId;
        public uint num;
        public string itemName;
        public uint cost;
        public uint stage;
        public uint intensify;
    }
    public class itemLotteryAwardInfoData
    {
        public string name;//中奖姓名
        public uint tpid;//道具配置id
        public uint cnt;//道具数量
        public uint tm;//中奖时间
        public uint intensify;//武器强化等级
        public uint stage;// 武器阶数


    }
    public enum LotteryType
    {
        CurrentInfo = 1,
        FreeDraw,
        IceDrawOnce,
        IceDrawTenth,
        NewBieDraw = 5,
        NewDrawInfo = 6,
        FreeTenth = 7,
        newDraw = 8
    }
}
