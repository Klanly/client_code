//using System;
//using System.Collections.Generic;
//using Cross;
//using GameFramework;

//namespace MuGame
//{
//    class HeroModel : ModelBase<HeroModel>
//    {
//        public static uint SET_ZX = 1;
//        public static uint CHANGE_ZX = 2;

//        private Dictionary<int, HeroXml> dHeroXml = new Dictionary<int, HeroXml>();
//        private Dictionary<int, HeroData> dHeroData = new Dictionary<int, HeroData>();
//        private List<HeroZXData> lHeroZx = new List<HeroZXData>(5);
//        public int curZxIdx = -1;

//        public Dictionary<int, int> notice_get_heroid = new Dictionary<int, int>();
//        public Dictionary<int, int> notice_strength_heroid = new Dictionary<int, int>();

//        public string curClickAddExpId = null; 
//        public bool isGetHeroOutZx = false; //用于玩家战斗力提升的屏蔽

//        public HeroModel()
//            : base()
//        {
//            initXml();

//            for (int i = 0; i < 5; i++)
//            {
//                lHeroZx.Add(new HeroZXData());
//            }
//        }

//        void initXml()
//        {
//            HeroXml txml;
//            SXML xml = XMLMgr.instance.GetSXML("hero.hero", null);
//            do
//            {
//                txml = new HeroXml();
//                txml.id = xml.getInt("id");
//                txml.name = xml.getString("hero_name");
//                txml.model_path = xml.getString("model_path");
//                txml.attackType = xml.getInt("hero_attack_type");
//                txml.avatarid = xml.getInt("obj");
//                dHeroXml.Add(txml.id, txml);
//            } while (xml.nextOne());
//        }


//        public Dictionary<int, HeroData> getHeros()
//        {
//            return dHeroData;
//        }

//        public void changeZx(int idx)
//        {
//            if (idx== curZxIdx)
//                return;

//            curZxIdx = idx;
//            dispatchEvent(GameEvent.Create(CHANGE_ZX, this, null));
//        }
//        public void setZx(int idx, HeroZXData zx, bool needEvent = true)
//        {

//            lHeroZx[idx] = zx;
//            if (needEvent)
//                dispatchEvent(GameEvent.Create(SET_ZX, this, null));
//        }

//        public List<HeroZXData> getZx()
//        {
//            return lHeroZx;
//        }

//        public List<int> getZxHerosId()
//        {
//            List<int> herosid = new List<int>();
//            foreach (HeroZXData data in lHeroZx)
//            {
//                foreach (HeroData hero in data.lHero)
//                {
//                    if (hero.petId > 0)
//                    {
//                        herosid.Add(hero.petId);
//                    }
//                }
//            }
//            return herosid;
//        }

//        public HeroZXData getCurZxData()
//        {
//            if (curZxIdx == -1)
//                return null;

//            return lHeroZx[curZxIdx];
//        }

//        public void addHero(HeroData data)
//        {
//            data.xml = getHeroDataById(data.petId);
//            if (dHeroData.ContainsKey(data.petId))
//            {
//                dHeroData[data.petId] = data;
//            }
//            else
//            {
//                dHeroData.Add(data.petId, data);
//            }
//        }
//        public HeroAttr getAttri(Variant info)
//        {
//            HeroAttr attr = new HeroAttr();

//            if (info.ContainsKey("battleAttrs"))
//            {
//                Variant attrs = info["battleAttrs"];
//                attr.max_hp = attrs["max_hp"];
//                attr.constitution = attrs["constitution"];
//                attr.strength = attrs["strength"];
//                attr.intelligence = attrs["intelligence"];
//                attr.physics = attrs["physics"];
//                attr.magic = attrs["magic"];
//                attr.physics_def = attrs["physics_def"];
//                attr.magic_def = attrs["magic_def"];
//                attr.critical_damage = attrs["critical_damage"];
//                attr.critical_def = attrs["critical_def"];
//                attr.ice_att = attrs["ice_att"];
//                attr.fire_att = attrs["fire_att"];
//                attr.thunder_att = attrs["thunder_att"];
//                attr.ice_def = attrs["ice_def"];
//                attr.fire_def = attrs["fire_def"];
//                attr.thunder_def = attrs["thunder_def"];
//            }
//            return attr;
//        }
//        public void strengthHero(Variant data)
//        {
//            int id = data["hero_id"];
//            if (dHeroData.ContainsKey(id))
//            {
//                HeroData one = new HeroData();
//                one.petId = data["hero_id"];
//                one.lv = data["level"];
//                one.exp = data["exp"];
//                one.hp = data["hp"];
//                one.strenglv = data["strengthen"];
//                one.quality = dHeroData[id].quality;
//                one.growth = dHeroData[id].growth;
//                one.character = dHeroData[id].character;

//                if (data.ContainsKey("combpt"))
//                {
//                    one.combpt = data["combpt"];
//                }

//                one.attr = HeroModel.getInstance().getAttri(data);

//                one.xml = getHeroDataById(one.petId);
//                dHeroData[id] = one;
//            }
//        }
//        public void addHeroToZx(Variant data)
//        {
//            if (data["left"])
//            {
//                lHeroZx[data["listNo"]].lHero[0] = dHeroData[data["petid"]];
//            }
//            else
//            {
//                lHeroZx[data["listNo"]].lHero[1] = dHeroData[data["petid"]];
//            }
//        }
//        public void recHeroToZx(int id)
//        {
//            foreach (HeroZXData zx in lHeroZx)
//            {
//                for (int i = 0; i < 2; i++)
//                {
//                    if (zx.lHero[i].petId == id)
//                    {
//                        zx.lHero[i] = new HeroData();
//                        break;
//                    }
//                }
//            }
//        }
//        public int getZxIndexById(int id)
//        {
//            int index = -1;
//            for (int j = 0; j < lHeroZx.Count;  j++)
//            {
//                for (int i = 0; i < 2; i++)
//                {
//                    if (lHeroZx[j].lHero[i].petId == id)
//                    {
//                        index = (j-1) * 2 + i;
//                        break;
//                    }
//                }
//            }
//            return index;
//        }
//        public HeroXml getHeroDataById(int id)
//        {
//            return dHeroXml[id];
//        }

//        public int getLockMaxLv()
//        {
//            int lv = 0;
//            SXML xml = XMLMgr.instance.GetSXML("hero.hero_location", "limit_type==1");
//            do 
//            {
//                if (xml.getInt("value") > lv)
//                {
//                    lv = xml.getInt("value");
//                }
//            } while (xml.nextOne());

//            return lv;
//        }

//        public int getLockMaxVipLv()
//        {
//            int viplv = 0;
//            SXML xml = XMLMgr.instance.GetSXML("hero.hero_location", "limit_type==2");
//            do
//            {
//                if (xml.getInt("value") > viplv)
//                {
//                    viplv = xml.getInt("value");
//                }
//            } while (xml.nextOne());

//            return viplv;
//        }
//        public void removeNotice(BagItemData item)
//        {
//            SXML xml = XMLMgr.instance.GetSXML("hero_system.hero_decompose", "item_id==" + item.id);
//            int id = xml.getInt("hero_id");

//            if (notice_get_heroid.ContainsKey(id))
//            {
//                notice_get_heroid.Remove(id);
//            }

//            if (notice_strength_heroid.ContainsKey(id))
//            {
//                notice_strength_heroid.Remove(id);
//            }
//        }
//        public bool checkNotice(BagItemData item)
//        {
//            SXML xml = XMLMgr.instance.GetSXML("hero_system.hero_decompose", "item_id=="+item.id);
//            int id = xml.getInt("hero_id");
//            if (id != -1)
//            {
//                if (dHeroData.ContainsKey(id))
//                {//提示强化 
//                    xml = XMLMgr.instance.GetSXML("hero_system.hero_strengthen", "hero_id==" + id);
//                    xml = xml.GetNode("num", "strengthen_level==" + (dHeroData[id].strenglv + 1));
//                    int neednum = xml.getInt("item_num");
//                    if (neednum != -1)  
//                    {
//                        if (item.num > neednum)
//                        {
//                            //IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_HERO);
//                            notice_strength_heroid[id] = 1;
//                        }
//                        else
//                        {
//                            if (notice_strength_heroid.ContainsKey(id))
//                            {
//                                notice_strength_heroid.Remove(id);
//                            }
//                        }
//                        if (notice_get_heroid.ContainsKey(id))
//                        {
//                            notice_get_heroid.Remove(id);
//                        }
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                else
//                {//提示召唤
//                    xml = XMLMgr.instance.GetSXML("hero_system.hero_decompose", "hero_id==" + id);
//                    int needNum = xml.getInt("num");
//                    if (needNum != -1)
//                    {
//                        if (item.num > needNum)
//                        {
//                            IconNoticeMgr.getInstance().showNotice(IconNoticeMgr.TYPE_HERO);
//                            notice_get_heroid[id] = 1;
//                        }
//                        else
//                        {
//                            if (notice_get_heroid.ContainsKey(id))
//                            {
//                                notice_get_heroid.Remove(id);
//                            }
//                        }
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//            }
//            return false;
//        }

//        public void checkVipHeroGetOut()
//        {
//            if (VipMgr.getValue(VipMgr.BATTLE_HERO_COUNT) < 3)
//            {
//                if (lHeroZx[3].lHero[1].petId > 0)
//                {
//                    HeroProxy.getInstance().sendRemPetTo(lHeroZx[3].lHero[1].petId);
//                }
//            }
//            if (VipMgr.getValue(VipMgr.BATTLE_HERO_COUNT) < 2)
//            {
//                if (lHeroZx[2].lHero[1].petId > 0)
//                {
//                    HeroProxy.getInstance().sendRemPetTo(lHeroZx[2].lHero[1].petId);
//                }
//            }
//            if (VipMgr.getValue(VipMgr.BATTLE_HERO_COUNT) < 1)
//            {
//                if (lHeroZx[1].lHero[1].petId > 0)
//                {
//                    HeroProxy.getInstance().sendRemPetTo(lHeroZx[1].lHero[1].petId);
//                }
//            }
//        }
//    }


//    public class HeroZXData
//    {
//        public List<HeroData> lHero;

//        public HeroZXData()
//        {
//            lHero = new List<HeroData>(2);
//            for (int i = 0; i < 2; i++)
//            {
//                lHero.Add(new HeroData());
//            }
//        }
//        public void setZx(int idx, HeroData d)
//        {

//            lHero[idx] = d;
//        }
//    }

//    public class HeroData
//    {
//        public int petId;
//        public int lv;
//        public int strenglv;
//        public int quality;
//        public int growth;
//        public int character;
//        public uint iid;
//        public HeroXml xml;
//        public int hp;
//        public int exp;
//        public int maxHp;
//        public HeroAttr attr;
//        public int combpt;
//    }
//    public class HeroAttr
//    {
//        public int max_hp;
//        public int constitution;
//        public int strength;
//        public int intelligence;
//        public int physics;
//        public int magic;
//        public int physics_def;
//        public int magic_def;
//        public int critical_damage;
//        public int critical_def;
//        public int ice_att;
//        public int fire_att;
//        public int thunder_att;
//        public int ice_def;
//        public int fire_def;
//        public int thunder_def;
//    }
//    public class HeroXml
//    {
//        public int id;
//        public int avatarid;
//        public string name;
//        public string model_path;
//        public int attackType;
//    }




//}
