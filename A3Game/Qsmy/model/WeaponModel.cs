using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
namespace MuGame
{
    class WeaponModel : ModelBase<WeaponModel>
    {
        public static uint EVENT_INIT_INFO = 0;

        public Dictionary<int, WeaponData> weaponFace;
        public Dictionary<int, cl_aryData> cl_arydata;
        public Dictionary<int, WeaponUpgradeData> upgradeData;
        public Dictionary<int, WeaponCastData> dicwcsdata;
        public int num = 0;
        public WeaponModel()
            : base()
        {
            weaponFace = new Dictionary<int, WeaponData>();
            cl_arydata = new Dictionary<int, cl_aryData>();
            upgradeData = new Dictionary<int, WeaponUpgradeData>();
            dicwcsdata = new Dictionary<int, WeaponCastData>();
        }


 
     
        public void addData(WeaponData one)
        {
           
            SXML s_xml = XMLMgr.instance.GetSXML("shadow_flare.shadow_flare", "id==" + one.id);
            if (s_xml != null)
            {
                one.name = s_xml.getString("name");
                one.attrId = s_xml.getInt("attrId");
                one.levelList = new List<WeaponLevelData>();
                SXML s_level = s_xml.GetNode("level", null);

                if (s_level != null)
                {
                    do
                    {
                        WeaponLevelData levelData = new WeaponLevelData();
                        //这边要输入确定升级界面的等级来确定所要增加的攻击力
                        levelData.id = s_level.getString("id");
                        levelData.que = s_level.getString("que");
                        one.levelList.Add(levelData);
                    } while (s_level.nextOne());
                }
            }
            weaponFace.Add(one.id,one);
        }


        public void getWeaponUpgradeDataById(int id)
        {
            SXML s_xml = XMLMgr.instance.GetSXML("shadow_flare.upgrade_exp", "id==" + id);

            WeaponUpgradeData data = new WeaponUpgradeData();
            data.id = id;

            if (s_xml != null)
            {
                data.expList = new List<UpgradeLevelData>();
                SXML s_upgrade = s_xml.GetNode("level", null);
                if (s_upgrade != null)
                {
                    do
                    {
                        UpgradeLevelData upgradeDatas = new UpgradeLevelData();
                        upgradeDatas.id = s_upgrade.getInt("id");
                        upgradeDatas.exp = s_upgrade.getInt("exp");
                        data.expList.Add(upgradeDatas);
                    } while (s_upgrade.nextOne());
                }
            }
            //upgradeData.Add(data.id, data); 
            upgradeData[data.id]= data;     
        }



        public WeaponUnlockData getWeaponUnlockDataById(string id)
        {
            WeaponUnlockData data = new WeaponUnlockData();
            data.id = id;
            SXML s_xml = XMLMgr.instance.GetSXML("shadow_flare.unlock", "id==" + id);
            if (s_xml != null)
            {
                data.unlock_lv = s_xml.getString("unlock_lv");
            }
            return data;
        }
        public WeaponCastData getWeaponCastDataById(int id)
        {
            WeaponCastData data = new WeaponCastData();
            SXML s_xml = XMLMgr.instance.GetSXML("cast_exp.", "shadow_flare==" + id);
            if (s_xml != null)
            {
                do
                {
                    data.material_id = s_xml.getInt("material_id");
                    data.cast_exp = s_xml.getInt("cast_exp");
                } while (s_xml.nextOne());
            }
            dicwcsdata[data.material_id]= data;

            return data;
        }


        public void tianjiaData(cl_aryData one)
        {
            
            cl_arydata.Add(one.id ,one);

        }

    }


    public class WeaponData
    {
        //武器cl-ID, 
        public int lv;
        public int exp;
        public int id;
        public string name;
        public int attrId;
        public List<WeaponLevelData> levelList;
        //public List<weaponUnlockData> lockList;
    }
    public class WeaponLevelData
    {
        //武器等级ID.攻击力
        public string id;
        public string que;
    }
    public class WeaponUpgradeData
    {        
        public int id;
        public List<UpgradeLevelData> expList;       
    }
    public class UpgradeLevelData
    { 
        // 武器等级ID和相对应的经验
        public int id;
        public int exp;
    }

    public class WeaponUnlockData
    {
        //武器ID,和相对应人物等级
        public string id;
        public string unlock_lv;
    }
    public class WeaponCastData
    {
        //种材料品质id和他们相对应的经验
        public int  material_id;
        public int cast_exp;
        public int shadow_flare;
    }


    public class cl_aryData
    {
        public int id;
        public int num;
    }
}