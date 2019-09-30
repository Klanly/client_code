using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable CheckNamespace
namespace MuGame
{
    public class A3_AchievementModel : ModelBase<A3_AchievementModel>
    {
        public static uint HAVE_REACH_ACHIEVEMENT = 0;

        public A3_AchievementModel()
            :base()
        {
            InitAchievementDic();
        }

        private List<SXML> achievementXML;
        public  List<SXML> AchievementXML
        {
            get
            {
                if (achievementXML == null)
                    achievementXML = XMLMgr.instance.GetSXMLList("achievement.achievement");

                return achievementXML;
            }
        }

        //所有成就数据
        private Dictionary<uint, AchievementData> dicAchievementData;

        //所有成就类型
        public List<uint> listCategory;

        //初始化所有成就数据
        private void InitAchievementDic()
        {
            dicAchievementData = new Dictionary<uint, AchievementData>();
            listCategory = new List<uint>();
            listCategory.Add(0);
            int length = AchievementXML.Count;
            for (int i = 0; i < length; i++)
            {
                AchievementData data = new AchievementData();

                data.id = AchievementXML[i].getUint("achievement_id");
                data.category = AchievementXML[i].getUint("category");
                data.name = AchievementXML[i].getString("name");
                data.type = AchievementXML[i].getUint("type");
                data.bndyb = AchievementXML[i].getUint("bndyb");
                data.point = AchievementXML[i].getUint("point");
                data.condition = AchievementXML[i].getUint("param1");
                data.value = AchievementXML[i].getUint("param2");
                data.desc = AchievementXML[i].getString("desc");
                data.degree = 0;
                
                if (!listCategory.Contains(data.category))
                    listCategory.Add(data.category);

                dicAchievementData[data.id] = data;
            }
        }

        //同步服务器信息//0表示未完成，1表示已完成未领奖，2表示已领奖-->
        public void SyncAchievementDataByServer(Variant v)
        {
            //TODO 从服务器同步成就进度
            if (v.ContainsKey("achives"))
            {
                List<Variant> list = v["achives"]._arr;
                foreach (Variant data in list)
                {
                    uint id = data["id"];
                    uint drgee = data["reach_num"];
                    uint state = data["state"];

                    if (dicAchievementData.ContainsKey(id))
                    {
                        dicAchievementData[id].degree = drgee;

                        dicAchievementData[id].state = (AchievementState)state;
                    }
                }
            }

            if (v.ContainsKey("ach_point"))
            {
                AchievementPoint = v["ach_point"];
				PlayerModel.getInstance().ach_point = v["ach_point"];
                a3_RankModel.nowexp = v["ach_point"];
            }
        }

        public string GetCategoryName(uint categoryId)
        {
            return ContMgr.getCont("achieve_tag" + categoryId);
        }

        //更新的成就ID 数组
        public List<uint> listAchievementChange = new List<uint>();
        //刚刚完成的成就
        public List<uint> listReachAchievent;

        //更新成就变化
        public void OnAchievementChangeFromServer(Variant v)
        {
            //TODO 成就变化
            if (v.ContainsKey("achives"))
            {
                List<Variant> list = v["achives"]._arr;

                foreach (Variant data in list)
                {
                    uint id = data["id"];
                    uint drgee = data["reach_num"];
                    uint state = data["state"];

                    if (!listAchievementChange.Contains(id))
                        listAchievementChange.Add(id);

                    if (dicAchievementData.ContainsKey(id))
                    {
                        dicAchievementData[id].degree = drgee;

                        dicAchievementData[id].state = (AchievementState)state;
                    }
                }
            }
        }

        //成就完成(变化)
        public void OnAchievementReachChange(Variant v)
        {
            if (v.ContainsKey("changed"))
            {
                listReachAchievent = new List<uint>();

                List<Variant> list = v["changed"]._arr;

                foreach (Variant data in list)
                {
                    uint id = data["id"];
                    uint drgee = data["reach_num"];
                    uint state = data["state"];

                    if (!listAchievementChange.Contains(id))
                        listAchievementChange.Add(id);

                    listReachAchievent.Add(id);

                    if (dicAchievementData.ContainsKey(id))
                    {
                        dicAchievementData[id].degree = drgee;

                        dicAchievementData[id].state = (AchievementState)state;
                    }
                }
            }
        }

        //领取成就奖励 id
        public uint GetAchievementID
        { get; set; }

        //获得成就奖励
        public void OnGetAchievePrize(Variant v)
        {
            AchievementPoint = v["ach_point"];
            GetAchievementID = v["ach_id"];

            dicAchievementData[GetAchievementID].state = AchievementState.RECEIVED;
            //TODO 处理奖励
        }

        //玩家成就点数
        private uint achievementPoint;
        public Action OnAchievementChange = null;
        public uint AchievementPoint
        {
            get { return achievementPoint; }
            set
            {
                if (achievementPoint > 0 && value - achievementPoint > 0)
                {
                    flytxt.instance.AddDelayFlytxt(ContMgr.getCont("achieve_get", new List<string> { (value - achievementPoint).ToString() }));
                    flytxt.instance.StartDelayFly(0.5f);
                }
                if(achievementPoint == value)
                    return;

                achievementPoint = value;

                if (OnAchievementChange != null)
                    OnAchievementChange();
            }
        }

        //通过类型选取相应的成就, 如果是0, 则返回全部成就信息
        public List<AchievementData> GetAchievenementDataByType(uint category)
        {
            //if (category == 0)
            //    return dicAchievementData.Values.ToList<AchievementData>();

            List<AchievementData> listData = new List<AchievementData>();
            
            List<AchievementData> listReached = new List<AchievementData>();
            List<AchievementData> listReceiced = new List<AchievementData>();
            List<AchievementData> listUnreached = new List<AchievementData>();
            foreach (uint key in dicAchievementData.Keys)
            {
                AchievementData data = dicAchievementData[key];

                if (category == 0 || data.category == category)
                {
                    switch (data.state)
                    {
                        case AchievementState.UNREACHED:
                            listUnreached.Add(data);
                            break;
                        case AchievementState.REACHED:
                            listReached.Add(data);
                            break;
                        case AchievementState.RECEIVED:
                            listReceiced.Add(data);
                            break;
                        default:
                            break;
                    }
                }
            }
            listData.AddRange(listReached);
            listData.AddRange(listUnreached);
            listData.AddRange(listReceiced);

            return listData;
        }

        //更具成就ID获取成就信息
        public AchievementData GetAchievementDataByID(uint id)
        {
            AchievementData data = null;

            if (dicAchievementData.ContainsKey(id))
                data = dicAchievementData[id];

            return data;
        }

        ////排序成就列表
        //private void SortAchievementDic()
        //{
        //    List<AchievementData> list = new List<AchievementData>();

        //    foreach (var item in dicAchievementData.Values)
        //    {
        //        list.in
        //    }
        //}

    }
    public class AchievementData
    {
        public uint id;//成就ID
        public uint category;//成就类ID
        public string name;//成就名字
        public string desc;//成就描述
        public uint type;//成就类型
        public uint bndyb;//绑定砖石/礼金
        public uint point;//成就点数
        public uint condition;//成就条件
        public uint value;//成就条件的指标
        public uint degree;//成就的进度

        public List<int> listParam;//成就参数

        public AchievementState state;
    }
    public enum AchievementState
    {
        UNREACHED = 0,
        REACHED = 1,
        RECEIVED = 2,
    }
}
