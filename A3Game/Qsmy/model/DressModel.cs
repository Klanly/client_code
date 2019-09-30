//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Cross;
//using GameFramework;


//namespace MuGame
//{
//    class DressModel : ModelBase<DressModel>
//    {
//        public List<DressData> list_dressHead = new List<DressData>();
//        public List<DressData> list_dressClothes = new List<DressData>();
//        public List<DressData> list_dressWeapon = new List<DressData>();
//        public List<DressData> list_dressBack = new List<DressData>();

//        public List<int> dressID = new List<int>();
//        public List<int> activedDressID = new List<int>();

//        public  Dictionary<int, DressData> dic_dressData = new Dictionary<int, DressData>();

//        public List<int> newType = new List<int>();
//        public Dictionary<int, int> canActiveType = new Dictionary<int, int>();
//        public bool isNoticed=false;
//        public Dictionary<int, List<strengthenData>> strengthen = new Dictionary<int, List<strengthenData>> ();
//        public Dictionary<int, int> dic_partMaxLv = new Dictionary<int, int>();//部位最大等级

//        public List<int> newDresslis = new List<int>();
//        public List<int> newDressitemlis = new List<int>();

//        bool isInited = false;
//        public void init()
//        {
//            //string actData = "";
//            //actData = FileMgr.loadString(FileMgr.TYPE_DRESS,"active");
//            //actData = actData.Replace(" ", "");
//            //if (actData != "" && actData!="error")
//            //{
//            //    try
//            //    {
//            //        string[] data = actData.Split('*');
//            //        foreach (string str in data)
//            //        {
//            //            if (str != "")
//            //            {
//            //                string[] arr = str.Split('#');
//            //                canActiveType.Add(int.Parse(arr[0]), int.Parse(arr[1]));
//            //                newType.Add(int.Parse(arr[0]));
//            //            }                        
//            //        }
//            //        isNoticed = true;
//            //    }
//            //    catch (System.Exception ex)
//            //    {
//            //        FileMgr.saveString(FileMgr.TYPE_DRESS, "active", "error");
//            //        newType.Clear();
//            //    }
//            //}
//            if (isInited)
//            {
//                return;
//            }
//            isInited = true;
            
//            int count = 0;
//            SXML dressSXML = XMLMgr.instance.GetSXML("dress.dress_info", null);
//            if (dressSXML == null)
//            {
//                return;
//            }
//            if (count != 0)
//            {
//                count = 0;
//            }
//            do
//            {
//                DressData data = new DressData();
//                data.id = dressSXML.getInt("id");
//                data.equipName = dressSXML.getString("equip_name");
//                data.dressType = dressSXML.getInt("dress_type");
//                data.quality = dressSXML.getInt("quality");
//                data.exp = dressSXML.getInt("dress2exp");
//                dic_dressData[data.id] = data;
//                dressID.Add(data.id);

//                int _type = data.dressType + 1;
//                switch (_type)
//                {
//                    case 1: list_dressWeapon.Add(data);
//                        count++;
//                        break;
//                    case 2: list_dressClothes.Add(data);
//                        count++;
//                        break;
//                    case 3: list_dressHead.Add(data);
//                        count++;
//                        break;
//                    case 4: list_dressBack.Add(data);
//                        count++;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            while (dressSXML.nextOne());

//            SXML strengthenSXML = XMLMgr.instance.GetSXML("dress.dress_strengthen", null);
//            if (strengthenSXML == null)
//                return;
//            do 
//            {
//                int _type = strengthenSXML.getInt("dress_type");
//                List<strengthenData> datalist = new List<strengthenData>();
//                SXML lvSxml = strengthenSXML.GetNode("dress_level", null);
//                do
//                {
//                    strengthenData sdata = new strengthenData();
//                    sdata.type = strengthenSXML.getInt("dress_type");
//                    sdata.maxLv = strengthenSXML.getInt("max_lv");
//                    sdata.lv = lvSxml.getInt("level");
//                    if (!dic_partMaxLv.ContainsKey(sdata.type))
//                        dic_partMaxLv.Add(sdata.type, sdata.maxLv);
//                    SXML lvSxml2 = strengthenSXML.GetNode("dress_level", "level==" + (sdata.lv + 1));
//                    SXML attSxml = lvSxml.GetNode("att", null);
//                    do
//                    {
//                        if (lvSxml2.hasFound)
//                            sdata.maxExp = lvSxml2.getInt("exp").ToString();
//                        else
//                            sdata.maxExp = "MAX";
//                        int tp = attSxml.getInt("att_type");
//                        switch (tp)
//                        {
//                            case 6: sdata.property.Add("冰攻");
//                                break;
//                            case 7: sdata.property.Add("火攻");
//                                break;
//                            case 8: sdata.property.Add("雷攻");
//                                break;
//                            case 9: sdata.property.Add("冰抗");
//                                break;
//                            case 10: sdata.property.Add("火抗");
//                                break;
//                            case 11: sdata.property.Add("雷抗");
//                                break;
//                            default:
//                                break;
//                        }
//                        sdata.value.Add(attSxml.getInt("att_value"));
//                    } while (attSxml.nextOne());
//                    datalist.Add(sdata);
//                } while (lvSxml.nextOne());
//                strengthen[_type] = datalist;
//            } while (strengthenSXML.nextOne());

//            foreach (int key in strengthen.Keys)
//            {
//                List<strengthenData> lis = strengthen[key];
//                int num = lis.Count-1;
//                int total = 0;
//                for (int i = 0; i < num; i++)
//                {
//                    total = total + int.Parse(lis[i].maxExp);
//                }
//                for (int i = 0; i < num;i++)
//                {
//                    if (i != 0)
//                        total=total-int.Parse(lis[i-1].maxExp);

//                    lis[i].totalExp = total;
//                }
//            }

//            DressProxy.getInstance().addEventListener(DressProxy.get_new_dress, showMarkNewDress);
//            DressProxy.getInstance().sendLoadDress();
//        }

//        public DressData getDataById(int id)
//        {
//            if (dic_dressData.ContainsKey(id))
//                return dic_dressData[id];
//            return null;
//        }

//        public strengthenData getStrData(int type,int lv)
//        {
//            return strengthen[type][lv - 1];
//        }

//        public float LvMaxExp(int lv)
//        {
//            return float.Parse(strengthen[0][lv - 1].maxExp);
//        }

//        public void showMarkNewDress(GameEvent e)
//        {
//            Variant data=e.data;
//            int id=data["dress_id"]._int32;
//            if (dic_dressData[id].isActive)
//                return;

//            int type = getDataById(id).dressType;
//            if (!newType.Contains(type))
//            {
//                newType.Add(type);
//            }
//            creatCanActiveType(type);
//            //saveLocalData(canActiveType);

//            if (!isNoticed)
//            {
//                isNoticed = true;
//            } 
//        }

//        public void creatCanActiveType(int type)
//        {
//            if (!canActiveType.ContainsKey(type))
//                canActiveType.Add(type, 1);
//            else
//                canActiveType[type] = canActiveType[type] + 1;
//        }

//        //public void saveLocalData(Dictionary<int, int> activeType)
//        //{
//        //    if (activeType.Count == 0)
//        //    {
//        //        FileMgr.saveString(FileMgr.TYPE_DRESS, "active", " ");
//        //        return;
//        //    } 
//        //    string str = "";
//        //    foreach (int type in activeType.Keys)
//        //    {
//        //        str = str + type.ToString() + "#" + activeType[type].ToString() + "*";
//        //    }
//        //    FileMgr.saveString(FileMgr.TYPE_DRESS, "active", str);
//        //}
//    }

//    class DressData
//    {
//        public int id;
//        public int dressType;
//        public int exp;
//        public int quality;
//        public string equipName;
//        public bool isActive = false;
//    }

//    class strengthenData
//    {
//        public int type;
//        public int lv;
//        public string maxExp;
//        public int maxLv;
//        public List<string> property=new List<string>();
//        public List<int> value=new List<int>();
//        public List<string> afterValue = new List<string>();
//        public int totalExp;
//    }
//}
