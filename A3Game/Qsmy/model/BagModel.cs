//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Cross;
//using GameFramework;

//namespace MuGame
//{
//    class BagModel : ModelBase<BagModel>
//    {
//        private Dictionary<string, BagItemData> Items;
//        public BagModel()
//            : base()
//        {
//            Items = new Dictionary<string, BagItemData>();
//        }
//        public void initItemList(List<Variant> arr)
//        {
//            if (Items == null)
//                Items = new Dictionary<string, BagItemData>();
//            foreach (Variant data in arr)
//            {
//                BagItemData itemData = new BagItemData();
//                itemData.id = data["tpid"];
//                itemData.num = data["cnt"];
//                addItem(itemData,false);
//            }
//        }
//        public void addItem(BagItemData data,bool needCheckHeroItem = true)
//        {
//            data.confdata = getItemDataById(data.id);

//            if (data.confdata.use_type == 13)
//            {
//                //if (Items.ContainsKey(data.id))
//                //{
//                //    if (Items[data.id].num < data.num)
//                //    {
//                //        DressModel.getInstance().newDressitemlis.Add(int.Parse(data.id));
//                //        DressProxy.getInstance().doOpenGet();
//                //    }
//                //}else if (getnew.isOn)
//                //{
//                //    DressModel.getInstance().newDressitemlis.Add(int.Parse(data.id));
//                //    DressProxy.getInstance().doOpenGet();
//                //}
//            }

//            if (Items.ContainsKey(data.id))
//            {
//                if (data.num == 0)
//                {
//                    Items.Remove(data.id);
//                }
//                else
//                {
//                    Items[data.id] = data;
//                }
//            }
//            else
//            {
//                Items.Add(data.id, data);
//            }

//            if (data.confdata.use_type == 33 && needCheckHeroItem)
//            {
//                HeroModel.getInstance().checkNotice(Items[data.id]);
//            }
//        }
//        public void remove(BagItemData data)
//        {
//            data.confdata = getItemDataById(data.id);
//            if (Items.ContainsKey(data.id))
//            {
//                if (data.num == 0)
//                {
//                    Items.Remove(data.id);
//                }
//                else
//                {
//                    Items[data.id] = data;
//                }
//            }
//        }
//        public void sellItem(uint id)
//        {
//            if (Items.ContainsKey(id.ToString()))
//            {
//                if (Items[id.ToString()].confdata.use_type == 33)
//                {
//                    HeroModel.getInstance().removeNotice(Items[Items[id.ToString()].id]);
//                }
//                Items.Remove(id.ToString());
//            }
//        }
//        public Dictionary<string, BagItemData> getItems()
//        {
//            return Items;
//        }

//        public int getItemNumById(string id)
//        {
//            if (Items.ContainsKey(id))
//            {
//                return Items[id].num;
//            }
//            return 0;
//        }
//        public ItemData getItemDataById(string id)
//        {
//            ItemData item = new ItemData();
//            item.id = id;
//            return item;
//        }
//        public Dictionary<string, BagItemData> getStrengthStones()
//        {
//            Dictionary<string, BagItemData> stones = new Dictionary<string, BagItemData>();

//            List<int> stones_id = new List<int>();

//            SXML s_xml = XMLMgr.instance.GetSXML("item.item", null);

//            foreach (BagItemData item in Items.Values)
//            {
//                if (item.confdata.intensify_score != -1)
//                {
//                    stones_id.Add(int.Parse(item.id));
//                }
//            }

//            stones_id.Sort();

//            foreach (int id in stones_id)
//            {
//                BagItemData stone = Items[id.ToString()];
//                stones.Add(id.ToString(), stone);
//            }

//            //stones.QSEQOrderBy();

//            return stones;
//        }
//        public int[] getStrengthStonesExp()
//        {
//            int[] exp = new int[5];
//            SXML s_xml = XMLMgr.instance.GetSXML("item.item",null);
//            int i = 0;
//            do
//            {
//                if (s_xml.getInt("intensify_score") != -1)
//                {
//                    exp[i] = s_xml.getInt("intensify_score");
//                    i++;
//                }
//            } while (s_xml.nextOne());
//            return exp;
//        }
//    }
//    public class BagItemData :BaseItemData{
//        //public string id;
//        //public int num;
//        public ItemData confdata;
//    }
//    public class ItemData {
//        public string id;
//        public string file;
//        public string borderfile;
//        public string item_name;
//        public string desc;
//        public string desc2;
//        public int quality;
//        public int value;
//        public int use_type;
//        public int intensify_score;
//        public int item_type;
//    }
//}
