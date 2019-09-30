using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable CheckNamespace
namespace MuGame
{
    public class A3_VipModel : ModelBase<A3_VipModel>
    {
        public List<uint> isGetVipGift = new List<uint>();
        //public Dictionary<int, int> itemData = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> giftdata = new Dictionary<int, Dictionary<int, int>>();
        public A3_VipModel():base()
        {
            if (VipLevelXML == null) return;

            InitVipAttData();
            InitVipGiftData();
            ReadXml_gidtData();
        }

        //vip等级XML
        private List<SXML> vipLevelXML;
        public List<SXML> VipLevelXML
        {
            get
            {
                if (vipLevelXML == null)
                    vipLevelXML = XMLMgr.instance.GetSXMLList("vip.viplevel");

                return vipLevelXML;
            }
        }

        public void viplvl_refresh() {
          A3_ActiveModel .getInstance().buy_cnt = A3_VipModel.getInstance().vip_exchange_num(7);
            expFb_count = vip_exchange_num(18);
            materialFb_count = vip_exchange_num(16);
            goldFb_count = vip_exchange_num(17);
            team_fb_1 = vip_exchange_num(23);


        }

        public int expFb_count = 0;
        public int materialFb_count = 0;
        public int goldFb_count = 0;
        public int team_fb_1 = 0;



        public int vip_exchange_num(int i)
        {
            int count = 0;
            SXML vipSXML = XMLMgr.instance.GetSXML("vip.viplevel", "vip_level==" + A3_VipModel.getInstance().Level);
            if (vipSXML != null)
            {
                SXML temp = vipSXML.GetNode("vt", "type=="+i);
                if (temp != null)
                {
                    count = temp.getInt("value");
                }
            }
            return count;
        }
        //vip礼包XMl
        private List<SXML> vipGiftXML;
        public List<SXML> VipGiftXML
        {
            get
            {
                if (vipGiftXML == null)
                    vipGiftXML = XMLMgr.instance.GetSXMLList("vip.gift");

                return vipGiftXML;
            }
        }

        //vip等级
        private int level = 0;
        public Action OnLevelChange = null;
        public int Level
        {
            get { return level; }
            set
            {
                if (level == value)
                    return;

                level = value;
                SelfRole._inst?.refreshVipLvl((uint)level);
                OnLevelChange?.Invoke();
                InterfaceMgr.doCommandByLua("VipModel:getInstance().modLevel", "model/VipModel", level);
            }
        }

        //vip经验
        private int exp = 0;
        public Action OnExpChange = null;
        public int Exp
        {
            get { return exp; }
            set
            {
                if (exp == value)
                    return;

                exp = value;


                if (OnExpChange != null)
                    OnExpChange();

            }
        }

        //当前等级最大经验
        public int GetCurLevelMaxExp()
        {
            int maxExp = 0;

            SXML xml = VipLevelXML[Level];

            if (xml != null)
                maxExp = xml.getInt("vip_point");

            return maxExp;
        }

        public int GetNextLvlMaxExp() 
        {
            int maxExp = 0;
            if (Level >= GetMaxVipLevel())
            {
                maxExp = -1;
            }
            else { 
                SXML xml = VipLevelXML[Level + 1];
                maxExp = xml.getInt("vip_point");
            }   
            return maxExp;
        }
        public int GetMaxVipLevel()
        {
            int maxLevel = 0;

            maxLevel = allVipData.Count - 1;

            return maxLevel;
        }
        public int GetPriMum() 
        {
            List<SXML> _lsxml = VipLevelXML[0].GetNodeList("vt");
            return _lsxml.Count;
        }
        public string Gettype_Name(int lvl ,int type) 
        {
            List<SXML> _lsxml = VipLevelXML[lvl].GetNodeList("vt");
            string name ="";
            if (_lsxml != null)
            {
                name = _lsxml[type].getString("show_name");
            }
            return name;
        }
        public string GetValue( int lvl, int type) 
        {
            List<SXML> _lsxml = VipLevelXML[lvl].GetNodeList("vt");
            string value = "";
            if (_lsxml != null)
            {
                value = _lsxml[type].getString("value");
            }
            return value;
        }
        public int GetShowType(int type) 
        {
            List<SXML> _lsxml = VipLevelXML[0].GetNodeList("vt");
            int showType = 0;
            if (_lsxml != null)
            {
                showType = _lsxml[type].getInt("show_type");
            }
            return showType;
        }

        public int GetType(int lvl,int type)
        {
            List<SXML> _lsxml = VipLevelXML[lvl].GetNodeList("vt");
            int Type = 0;
            if (_lsxml != null)
            {
                Type = _lsxml[type].getInt("type");
            }
            return Type;
        }
        //vip属性数据
        private List<List<int>> allVipData = new List<List<int>>();
        private void InitVipAttData()
        {
            int length = VipLevelXML.Count;
            for (int i = 0; i < length; i++)
            {
                List<int> vipAtt = new List<int>();

                List<SXML> _lsxml = VipLevelXML[i].GetNodeList("vt");
                if (_lsxml != null)
                {
                    for (int j = 0; j < _lsxml.Count; j++)
                    {
                        vipAtt.Add(_lsxml[j].getInt("value"));
                    }
                }
                else
                {
                    vipAtt.Add(-1);
                }
               
                allVipData.Add(vipAtt);
            }
        }

        //vip礼包数据
        private List<int> allVipGiftData = new List<int>();
        private void InitVipGiftData()
        {
            int length = VipGiftXML.Count;
            for (int i = 0; i < length; i++)
            {
                List<SXML> _lsxml = VipGiftXML[i].GetNodeList("item");

                if (_lsxml != null)
                {
                    allVipGiftData.Add(_lsxml[0].getInt("item_id"));
                }
                else
                {
                    allVipGiftData.Add(-1);
                }
            }
        }
        private void ReadXml_gidtData()
        {
            foreach (int i in allVipGiftData) 
            {
                if (i <= 0) continue;
                else
                {
                    SXML xml = XMLMgr.instance.GetSXML("itemdrop.itemdrop", "id==" + i);
                    SXML itempkg = xml.GetNode("itempkg");
                    int carr = itempkg.getInt("carr");
                    SXML itempkg_m;
                    if (carr > 0)
                    {
                        itempkg_m = xml.GetNode("itempkg", "carr==" + PlayerModel.getInstance().profession);
                    }
                    else {
                        itempkg_m = xml.GetNode("itempkg");
                    }
                    List<SXML> item_group = itempkg_m.GetNodeList("group");
                    Dictionary <int ,int> item = new Dictionary<int,int> ();
                    foreach (SXML it in item_group)
                    {
                        int id = 0;
                        int count = 0;
                        SXML itm = it.GetNode("itm");
                        SXML eqp = it.GetNode("eqp");
                        if (itm != null)
                        {
                             id = itm.getInt("itemid");
                             count = itm.getInt("max");
                        }
                        if (eqp!=null) 
                        {
                            id = eqp.getInt("itemid");
                            count = eqp.getInt("max");
                        }
                        item[id] = count;
                    }
                    giftdata[i] = item;
                }
            }
            
           
        }

        private Dictionary<int, List<int>> allVipPriData = new Dictionary<int,List<int>>();
        private void InitVipPriData() 
        {
            //SXML sxml = XMLMgr.instance
        }
        //活动vip属性列表
        public List<int> GetVipAttByLevel(int level)
        {
            List<int> list = new List<int>();
            list = allVipData[level];

            return list;
        }
        //获得指定vip等级所需充值宝石量
        public int GetVipPoint(int level) 
        {

            return 1;
        }
        //获得指定vip奖励列表
        public int GetVipGiftListByLevel(int level)
        {
            int list;
            list = allVipGiftData[level];

            return list;
        }

        public string GetVipState()
        { 
            string str = null;

            SXML xml = XMLMgr.instance.GetSXML("vip.vip_state");
            if (xml != null)
            {
                str = xml.getString("state_str");
            }

            return str;
        }

    }
    class GiftData 
    {

    }
}
