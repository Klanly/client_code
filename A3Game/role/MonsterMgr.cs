using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using MuGame;
using Cross;
//using MuGame.role.monster;

public class MonsterMgr : GameEventDispatcher
{
    public static uint EVENT_MONSTER_ADD = 1;
    public static uint EVENT_MONSTER_REMOVED = 2;
    public static uint EVENT_ROLE_BORN = 3;

    static public MonsterMgr _inst = new MonsterMgr();
    public TaskMonId taskMonId;
    public List<Variant> cacheProxy = new List<Variant>();
    public List<Variant> cacheProxy_pvp = new List<Variant>();

    public List<MonsterRole> m_listMonster = new List<MonsterRole>();
    public Dictionary<uint, MonsterRole> m_mapMonster = new Dictionary<uint, MonsterRole>();
    public Dictionary<uint, MonsterRole> m_mapFakeMonster = new Dictionary<uint, MonsterRole>();
    // public Dictionary<uint, MonsterPlayer> m_mapOtherPlayer = new Dictionary<uint, MonsterPlayer>();

    public Dictionary<uint, uint> roleSummonMapping = new Dictionary<uint, uint>();

    private uint idIdx = 1;

    public delegate Type typeDelegate(string str);
    public static typeDelegate getTypeHandle;

    private Dictionary<int, SXML> dMon;

    public Dictionary<string, MonEffData> dMonEff;
    public void init()
    {
        if (dMon != null)
            return;

        dMon = new Dictionary<int, SXML>();
        List<SXML> l = XMLMgr.instance.GetSXMLList("monsters.monsters");
        foreach (SXML xml in l)
        {
            dMon[xml.getInt("id")] = xml;
        }

        if (dMonEff != null)
            return;

        dMonEff = new Dictionary<string, MonEffData>();
        List<SXML> xmleff = XMLMgr.instance.GetSXMLList("effect.eff");
        foreach (SXML xml in xmleff)
        {
            MonEffData one = new MonEffData();
            one.id = xml.getString("id");
            one.romote = xml.getInt("romote") == 1;
            one.Lockpos = xml.getInt("romote") == 2;
            one.file = xml.getString("file");
            one.y = xml.getFloat("y");
            one.f = xml.getFloat("f");
            one.sound = xml.getString("sound");
            one.rotation = xml.getFloat("rotation");
            if (xml.getFloat("speed") == -1)
                one.speed = 1;
            else
                one.speed = xml.getFloat("speed");
            dMonEff[one.id] = one;
        }
    }


    public MonsterRole AddMonster(Variant m,bool invisible = true)
    {
        if (GRMap.grmap_loading)
        {
            cacheProxy.Add(m);
            return null;
        }




        int num = 0;
        if (m.ContainsKey("boset_num"))
        {
            num = m["boset_num"];
        }
        Vector3 born_pt = new Vector3(m["x"] / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, m["y"] / GameConstant.PIXEL_TRANS_UNITYPOS);
        string monName = null;
        if (m.ContainsKey("owner_name"))
            monName = m["owner_name"];
        MonsterRole role = _inst.AddMonster(m["mid"], born_pt, m["iid"], m["face"], num, name: monName);
        if (role != null)
        {
            role.curhp = m["hp"];
            role.maxHp = m["battleAttrs"]["max_hp"];
            role.invisible = invisible;
            if (!invisible) role.m_curGameObj.SetActive(false);
        }

        if (m.ContainsKey("sprite_flag"))
        {
            //屏蔽掉怪物异步加载后报错的无用代码
            //uint call = m["sprite_flag"];
            //uint iid = m["iid"];
            //var vv = MonsterMgr._inst.getMonster(iid);
            //if (vv != null)
            //{
            //    Transform body = vv.m_curModel.FindChild("body");
            //    if (body != null)
            //    {
            //        SkinnedMeshRenderer render = body.GetComponent<SkinnedMeshRenderer>();
            //        if (render != null)
            //        {
            //            switch (call)
            //            {
            //                case 0:
            //                    foreach (var v in render.sharedMaterials)
            //                    {
            //                        //v.SetColor("_RimColor", new Color(0, 0, 0, 0));
            //                        //v.SetFloat("_RimWidth", 0f);
            //                    }
            //                    break;
            //                case 1:
            //                    //render.sharedMaterial = U3DAPI.U3DResLoad<Material>("default/monster_1021_heite_gold");
            //                    break;
            //            }
            //        }
            //    }
            //}
        }

        if (m.ContainsKey("moving"))
        {
            role.to_moving = true;
            role.to_x = m["moving"]["to_x"]._float;
            role.to_y = m["moving"]["to_y"]._float;
        }

        return role;
    }
    public void AddMonster_PVP(Variant p)
    {
        init();
        if (GRMap.grmap_loading)
        {
            cacheProxy.Add(p);
            return;
        }
        uint iid = p["iid"]._uint;
        uint cid = 0;
        int type = p["carr"];
        if (p.ContainsKey("cid"))
        { cid = p["cid"]._uint; }
        string name = p["name"];
        Vector3 born_pt = new Vector3(p["x"] / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, p["y"] / GameConstant.PIXEL_TRANS_UNITYPOS);
        MonsterPlayer pro_role = null;
        //Type TCls = null;

       

        if (2 == type)
        {
            pro_role = new ohterP2Warrior();
        }
        else if (3 == type)
        {
            pro_role = new ohterP3Mage();
        }
        else if (5 == type)
        {
            pro_role = new ohterP5Assassin();
        }

        if (iid > 0)
        {
            pro_role.m_unIID = iid;
            m_mapMonster.Add(iid, pro_role);
        }
        else
        {
            pro_role.isfake = true;
            pro_role.m_unIID = idIdx;
            m_mapFakeMonster.Add(idIdx, pro_role);

            idIdx++;
        }

        if (2 == type)
        {
            (pro_role as ohterP2Warrior).Init(name, EnumLayer.LM_OTHERPLAYER, born_pt);
            //TCls = System.Type.GetType("M000P2");
        }
        else if (3 == type)
        {
            (pro_role as ohterP3Mage).Init(name, EnumLayer.LM_OTHERPLAYER, born_pt);
            //TCls = System.Type.GetType("M000P3");
        }
        else if (5 == type)
        {
            (pro_role as ohterP5Assassin).Init(name, EnumLayer.LM_OTHERPLAYER, born_pt);
            //System.Type.GetType("M000P5");
        }

        //if (TCls == null)
        //{
        //    TCls = System.Type.GetType("M00000");
        //}

        //pro_role = (MonsterPlayer)Activator.CreateInstance(TCls);
        pro_role.m_curModel.position = born_pt;
        if (p.ContainsKey("face"))
        {
            pro_role.m_curModel.rotation = new Quaternion(pro_role.m_curModel.rotation.x, p["face"], pro_role.m_curModel.rotation.z, 0);
        }
        if (pro_role == null)
            return;
        // m_mapOtherPlayer.Add(iid, pro_role);
        pro_role.setPos(born_pt);
        pro_role.SetDestPos(born_pt);
        pro_role.m_unIID = iid;
        pro_role.m_unCID = cid;
        pro_role.m_unPK_Param = cid;
        pro_role.maxHp = p["battleAttrs"]["max_hp"];
        pro_role.curhp = p["hp"];
        if (p.ContainsKey("zhuan"))
            pro_role.zhuan = p["zhuan"];
        if (p.ContainsKey("pk_state"))
            pro_role.Pk_state = p["pk_state"];
        switch (pro_role.Pk_state)
        {
            case 0:
                pro_role.m_ePK_Type = PK_TYPE.PK_PEACE;
                break;
            case 1:
                pro_role.m_ePK_Type = PK_TYPE.PK_PKALL;
                break;
            case 2:
                pro_role.m_ePK_Type = PK_TYPE.PK_TEAM;
                break;
            case 3:
                pro_role.m_ePK_Type = PK_TYPE.PK_LEGION;
                break;
            case 4:
                pro_role.m_ePK_Type = PK_TYPE.PK_PEACE;
                break;
        };
        if (p.ContainsKey("teamid"))
        {
            pro_role.m_unTeamID = p["teamid"];
        }
        if (p.ContainsKey("clanid"))
        {
            pro_role.m_unLegionID = p["clanid"];
        }
        //if (p.ContainsKey("pet"))
        //{
        //    pro_role.ChangePetAvatar(p["pet"]["id"], p["pet"]["stage"]);
        //}
        pro_role.creatPetAvatar(type);
        pro_role.roleName = p["name"];

        //if (GRMap.grmap_loading == false)
        //    pro_role.refreshViewType(1);

        //if (Globle.isHardDemo)
        //    TCls = getTypeHandle("Md" + tempid);
        //else
        //{
        //    if (isCollect)
        //    {
        //        TCls = System.Type.GetType("CollectRole");
        //    }
        //    else
        //    {
        //        TCls = System.Type.GetType("M" + tempid);
        //    }
        //}
        //if (id == 4002 || carr == 2) TCls = System.Type.GetType("M000P2");
        //else if (id == 4003 || carr == 3) TCls = System.Type.GetType("M000P3");
        //else if (id == 4005 || carr == 5) TCls = System.Type.GetType("M000P5");

        //Type TCls;
        //if (Globle.isHardDemo)
        //    TCls = getTypeHandle("Md" + iid);
        //else
        //{
        //    TCls = System.Type.GetType("M" + iid);

        //}
        //if (TCls == null)
        //{
        //    TCls = System.Type.GetType("M00000");
        //}

        //pro_role = (MonsterPlayer)Activator.CreateInstance(TCls);

        pro_role.refreshViewData1(p);

    }

    public MonsterRole AddSummon(Variant m)
    {
        init();
        if (GRMap.grmap_loading)
        {
            cacheProxy.Add(m);
            return null;
        }

        Vector3 born_pt = new Vector3(m["x"] / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, m["y"] / GameConstant.PIXEL_TRANS_UNITYPOS);
        int id = m["mid"];
        uint serverid = m["iid"];
        if (m_mapMonster.ContainsKey(serverid))
        {
            if (m["owner_cid"] == PlayerModel.getInstance().cid)
            {
                if (a3_herohead.instance)
                {
                    A3_SummonModel.getInstance().lastatkID = 0;
                    a3_herohead.instance.refresh_sumHp(m["hp"], m["battleAttrs"]["max_hp"]);
                    a3_herohead.instance.refresh_sumbar();
                    a3_herohead.instance.do_sum_CD = false;
                }
            }
            return m_mapMonster[serverid];
        }

        //return null;

        SXML xml = dMon[id];
        int tempid = xml.getInt("obj");
        float scale = xml.getFloat("scale");
        string name = xml.getString("name");

        if (serverid <= 0)
        {
            if (Globle.m_nTestMonsterID > 0)
                tempid = Globle.m_nTestMonsterID;
        }

        MS0000 mon = new MS0000();

        mon.tempXMl = xml;

        mon.isBoos = xml.getInt("boss") == 1;
        mon.isBoss_c = xml.getInt("boss_c") == 1;
        if (scale > 0f)
            mon.scale = scale;

        if (mon != null)
        {
            if (serverid > 0)
            {
                mon.m_unIID = serverid;
                m_mapMonster.Add(serverid, mon);
                roleSummonMapping[ m[ "owner_cid" ] ] =  serverid ; // 人物对应的召唤兽 id
            }
            else
            {
                mon.isfake = true;
                mon.m_unIID = idIdx;
                m_mapFakeMonster.Add(idIdx, mon);
                idIdx++;
            }

            mon.masterid = m["owner_cid"];
            mon.issummon = true;
            mon.summonid = id;

            if (SceneCamera.m_nModelDetail_Level != 1 && mon.masterid != PlayerModel.getInstance().cid)
            {//设置隐藏召唤兽
                mon.Init("monster_" + tempid, EnumLayer.LM_DEFAULT, born_pt, 0);
            }
            else
            {
                mon.Init("monster_" + tempid, EnumLayer.LM_MONSTER, born_pt, 0);
            }
            mon.m_layer = EnumLayer.LM_MONSTER;

            PlayerNameUIMgr.getInstance().show(mon);
            PlayerNameUIMgr.getInstance().setName(mon, name, m["owner_name"] + ContMgr.getCont("MonsterMgr"));
            mon.roleName = name;
            mon.monsterid = id;

            //if (GRMap.grmap_loading == false)
            //    mon.refreshViewType(2);
           

            //mon.master = RoleMgr._instance.getRole(m["owner_cid"]);
            
            if (mon.masterid == PlayerModel.getInstance().cid)
            {
                if (a3_herohead.instance)
                {
                    A3_SummonModel.getInstance().lastatkID = 0;
                    a3_herohead.instance.refresh_sumHp(m["hp"], m["battleAttrs"]["max_hp"]);
                    a3_herohead.instance.refresh_sumbar();
                    a3_herohead.instance.do_sum_CD = false;
                }
            }
        }
        m_listMonster.Add(mon);

        if (mon != null)
            dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_ADD, this, mon));

        if (mon != null)
        {
            mon.curhp = m["hp"];
            mon.maxHp = m["battleAttrs"]["max_hp"];
            mon.owner_cid = m["owner_cid"];
        }
        return mon;
    }

    public MonsterRole AddDartCar(Variant d)//镖车
    {
        init();
        if (GRMap.grmap_loading)
        {
            cacheProxy.Add(d);
            return null;
        }
        Vector3 born_pt = new Vector3(d["x"] / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, d["y"] / GameConstant.PIXEL_TRANS_UNITYPOS);
        int id = d["mid"];
        uint serverid = d["iid"];
        if (m_mapMonster.ContainsKey(serverid))
            return m_mapMonster[serverid];

        SXML xml = dMon[id];
        int tempid = xml.getInt("obj");
        float scale = xml.getFloat("scale");
        string name = string.Empty;

        if (serverid <= 0)
        {
            if (Globle.m_nTestMonsterID > 0)
                tempid = Globle.m_nTestMonsterID;
        }
        MDC000 mon = new MDC000();
        mon.tempXMl = xml;
        mon.isBoos = xml.getInt("boss") == 1;
        mon.isBoss_c = xml.getInt("boss_c") == 1;
        if (scale > 0f)
            mon.scale = scale;
        if (mon != null)
        {
            if (serverid > 0)
            {
                mon.m_unIID = serverid;
                m_mapMonster.Add(serverid, mon);//这个地图上的所有的monster
            }
            else
            {
                mon.isfake = true;
                mon.m_unIID = idIdx;
                m_mapFakeMonster.Add(idIdx, mon);
                idIdx++;
            }

            mon.Init("monster_" + tempid, EnumLayer.LM_MONSTER, born_pt, 0);
            mon.curhp = d["hp"];
            mon.maxHp = d["battleAttrs"]["max_hp"];
            mon.escort_name = d["escort_name"];
            PlayerNameUIMgr.getInstance().show(mon);
            PlayerNameUIMgr.getInstance().setDartName(mon, d["escort_name"] + ContMgr.getCont("MonsterMgr1"));
            mon.roleName = d["escort_name"] + ContMgr.getCont("MonsterMgr1");
            mon.monsterid = id;
            if (mon.roleName == A3_LegionModel.getInstance().myLegion.name)
            {
                mon.m_isMarked = false;
            }

            //if (GRMap.grmap_loading == false)
            //    mon.refreshViewType(2);

            mon.dartid = id;
            mon.isDart = true;
        }
        m_listMonster.Add(mon);
        if (mon != null)
            dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_ADD, this, mon));


        if (d.ContainsKey("moving"))
        {
            uint iid = d["iid"]._uint;
            var vv = MonsterMgr._inst.getMonster(iid);
            float to_x = d["moving"]["to_x"]._float;
            float to_y = d["moving"]["to_y"]._float;

            NavMeshHit hit;
            Vector3 vec = new Vector3(to_x * 32 / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, to_y * 32 / GameConstant.PIXEL_TRANS_UNITYPOS);
            NavMesh.SamplePosition(vec, out hit, 100f, vv.m_layer);
            vv.SetDestPos(hit.position);
        }
        return mon;
    }

    public MonsterRole AddMonster(int id, Vector3 pos, uint serverid = 0, float roatate = 0, int boset_num = 0, int carr = 0,string name=null)
    {
        init();
        if (m_mapMonster.ContainsKey(serverid))
            return m_mapMonster[serverid];

        MonsterRole mon = null;
        //if (monID == 1)
        //{
        SXML xml = dMon[id];
        int tempid = xml.getInt("obj");
        float scale = xml.getFloat("scale");

        //用来给美术测试新加的怪物用的
        if (serverid <= 0)
        {
            if (Globle.m_nTestMonsterID > 0)
                tempid = Globle.m_nTestMonsterID;
        }

        bool isCollect = xml.getInt("collect_tar") > 0;

        bool isboxCollect = xml.getInt("collect_box") > 0;   // 判断这个宝箱是不是采集用的    > 0 则是采集的用宝箱

        //Type TCls;
        //if (Globle.isHardDemo)
        //    TCls = getTypeHandle("Md" + tempid);
        //else
        //{
        //    if (isCollect)
        //    {
        //        if (tempid == 122 && !isboxCollect)
        //        {
        //            TCls = System.Type.GetType("CollectBox");
        //        }
        //        else TCls = System.Type.GetType("CollectRole");
        //    }
        //    else
        //    {
        //        TCls = System.Type.GetType("M" + tempid);
        //    }
        //}
        //if (id == 4002 || carr == 2) TCls = System.Type.GetType("M000P2");
        //else if (id == 4003 || carr == 3) TCls = System.Type.GetType("M000P3");
        //else if (id == 4005 || carr == 5) TCls = System.Type.GetType("M000P5");

        //if (TCls == null)
        //{
        //    TCls = System.Type.GetType("M00000");
        //}

        //mon = (MonsterRole)Activator.CreateInstance(TCls);

        if (isCollect)
        {
            if (tempid == 122)
                mon = new CollectBox();
            else
                mon = new CollectRole();
        }
        if (id == 4002 || carr == 2) mon = new M000P2();
        else if (id == 4003 || carr == 3) mon = new M000P3();
        else if (id == 4005 || carr == 5) mon = new M000P5();
        if (mon == null)
            mon = new M00000();


        if (name != null)
            mon.ownerName = name;
        if (carr == 0)
        { mon.roleName = xml.getString("name"); }
        mon.tempXMl = xml;

        mon.m_circle_type = xml.getInt("boss_circle");
        if (xml.getFloat("boss_circle_scale") == -1)
            mon.m_circle_scale = 1;
        else
            mon.m_circle_scale = xml.getFloat("boss_circle_scale");

        mon.isBoos = xml.getInt("boss") == 1;
        mon.isBoss_c = xml.getInt("boss_c") == 1;
        if (scale > 0f)
            mon.scale = scale;

        if (serverid > 0)
        {
            mon.m_unIID = serverid;
            m_mapMonster.Add(serverid, mon);
        }
        else
        {
            mon.isfake = true;
            mon.m_unIID = idIdx;
            m_mapFakeMonster.Add(idIdx, mon);

            idIdx++;
        }

        if (mon != null)
        {
            if (id == 4002 || carr == 2)
            {
                mon.Init("profession_warrior_inst", EnumLayer.LM_MONSTER, pos, roatate);
            }
            else if (id == 4003 || carr == 3)
            {
                mon.Init("profession_mage_inst", EnumLayer.LM_MONSTER, pos, roatate);
            }
            else if (id == 4005 || carr == 5)
            {
                mon.Init("profession_assa_inst", EnumLayer.LM_MONSTER, pos, roatate);
            }
            else if (isCollect)
            {
                mon.Init("npc_" + tempid, EnumLayer.LM_MONSTER, pos, roatate);
            }
            else
            {
                mon.Init("monster_" + tempid, EnumLayer.LM_MONSTER, pos, roatate);
            }
            mon.monsterid = id;

            if (boset_num > 0)
            {
                PlayerNameUIMgr.getInstance().show(mon);
                PlayerNameUIMgr.getInstance().seticon_forDaobao(mon, boset_num);
            }

            if (A3_ActiveModel.getInstance().mwlr_map_info.Count > 0)
            {
                PlayerNameUIMgr.getInstance().seticon_forMonsterHunter(mon, A3_ActiveModel.getInstance().mwlr_map_info[0]["target_mid"] != id);
            }

          
            //if (GRMap.grmap_loading == false)
            //    mon.refreshViewType(2);
        }


        m_listMonster.Add(mon);


        //}
        //else if (monID == 2)
        //{
        //    mon = new M10002Srg();

        //    mon.Init("monster/10002", EnumLayer.LM_MONSTER, bornpt);
        //    m_mapMonster.Add(idIdx, mon);
        //}
        //else if (monID == 3)
        //{
        //    mon = new M10003Stl();

        //    mon.Init("monster/10003", EnumLayer.LM_MONSTER, bornpt);
        //    m_mapMonster.Add(idIdx, mon);
        //}
        //if (mon != null)
        //{
        //    MonsterData dta = new MonsterData();
        //    dta.roleId = idIdx;
        //    dta.monid = monID;
        //    mon.monsterDta = dta;
        //}

        //idIdx++;

        if (mon != null)
            dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_ADD, this, mon));

        return mon;
    }

    public void RemoveMonster(MonsterRole role)
    {
        if (!role.isfake)
        {
            if (!m_mapMonster.ContainsKey(role.m_unIID))
                return;
            MonsterRole mon = m_mapMonster[role.m_unIID];

            mon.dispose();
            m_mapMonster.Remove(role.m_unIID);
        }
        else
        {
            if (!m_mapFakeMonster.ContainsKey(role.m_unIID))
                return;
            MonsterRole mon = m_mapFakeMonster[role.m_unIID];

            mon.dispose();
            m_mapFakeMonster.Remove(role.m_unIID);
        }
        if (a3_liteMiniBaseMap.instance != null)
            a3_liteMiniBaseMap.instance.removeRoleInMiniMap(role.strIID);
        m_listMonster.Remove(role);
        dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_REMOVED, this, role));
    }
    public void HideInvaildMonster()
    {
        foreach (MonsterRole m in m_listMonster)
        {
            if (TeamProxy.getInstance().MyTeamData == null)
                if (m.ownerName != null && !m.ownerName.Equals(PlayerModel.getInstance().name))
                    m.m_curGameObj.SetActive(false);
            if (TeamProxy.getInstance().MyTeamData != null)
                if (m.ownerName != null && !TeamProxy.getInstance().MyTeamData.IsInMyTeam(m.ownerName))
                    m.m_curGameObj.SetActive(false);
        }
    }
    public void RefreshVaildMonster()
    {
        if (TeamProxy.getInstance().MyTeamData != null)
            foreach (MonsterRole m in m_listMonster)
                if (m.ownerName != null && TeamProxy.getInstance().MyTeamData.IsInMyTeam(m.ownerName))
                    m.m_curGameObj.SetActive(true);
    }
    public void RemoveMonster(uint iid)
    {
        if (m_mapMonster.ContainsKey(iid))
        {
            RemoveMonster(m_mapMonster[iid]);
        }
    }

    public void onMapLoaded()
    {
        //foreach (MonsterRole role in m_mapMonster.Values)
        //{
        //    role.refreshViewType(2);
        //}

        foreach (Variant v in cacheProxy)
        {
            if (v.ContainsKey("carr"))
            {
                AddMonster_PVP(v);
            }
            else if (v.ContainsKey("owner_cid"))
            {
                AddSummon(v);
            }
            else if (v.ContainsKey("escort_name"))
            {
                if (PlayerModel.getInstance().up_lvl>=1)
                {
                    AddDartCar(v);
                }
                
            }
            else
            {
                AddMonster(v);
            }

        }


        foreach (Variant v in cacheProxy_pvp)
        {

        }
        cacheProxy.Clear();
        cacheProxy_pvp.Clear();
    }

    public MonsterRole FindWhoPhy(Transform phy, bool includeCollect = false)
    {
        foreach (MonsterRole p in m_mapMonster.Values)
        {
            if (!includeCollect && p is CollectRole) continue;

            if (p.isDead) continue;
            //if (p.m_eFight_Side != FIGHT_A3_SIDE.FA3S_ENEMYOTHER) continue;

            if (p.m_curPhy == phy)
            {
                return p;
            }
        }

        foreach (MonsterRole p in m_mapFakeMonster.Values)
        {
            if (p.isDead || p is CollectRole) continue;
            //if (p.m_eFight_Side != FIGHT_A3_SIDE.FA3S_ENEMYOTHER) continue;

            if (p.m_curPhy == phy)
            {
                return p;
            }
        }

        return null;
    }

    public MonsterRole getServerMonster(uint iid)
    {
        if (m_mapMonster.ContainsKey(iid))
            return m_mapMonster[iid];
        return null;
    }

    public MonsterRole getFakeMonster(uint iid)
    {
        if (m_mapFakeMonster.ContainsKey(iid))
            return m_mapFakeMonster[iid];
        return null;
    }


    public MonsterRole getMonster(uint iid)
    {
        MonsterRole role = getServerMonster(iid);
        if (role != null)
            return role;
        return getFakeMonster(iid);
    }

    public bool HasMonInView()
    {
        bool has = false;
        var etor = m_mapMonster.GetEnumerator();
        while (etor.MoveNext())
        {
            if (etor.Current.Value.isDead || etor.Current.Value is CollectRole)
                continue;

            has = true;
            break;
        }
        return has;
    }

    public MonsterRole FindNearestMonster(Vector3 pos, Func<MonsterRole, bool> handle = null, bool useMark = false, PK_TYPE pkState = PK_TYPE.PK_PEACE, bool onTask = false)
    {
        float dis = 9999999f;

        MonsterRole role = null,t_role = null;
        float t_dis;
        RoleMgr.ClearMark(!useMark, pkState,
            filterHandle: (m) => (m.m_curPhy.position - pos).magnitude < (SelfRole.fsm.Autofighting ? StateInit.Instance.Distance : SelfRole._inst.m_LockDis));
        foreach (MonsterRole m in m_mapMonster.Values)
        {
            if (m.isDead || m is CollectRole || (m is MS0000 && ((MS0000)m).owner_cid == PlayerModel.getInstance().cid)) continue;
            if (m.issummon) continue;
            if (SelfRole.fsm.Autofighting && (m.m_curPhy.position - StateInit.Instance.Origin).magnitude > StateInit.Instance.Distance) continue;
            else {
                float _off_dis = (m.m_curPhy.position - pos).magnitude;
                if (_off_dis < (SelfRole.fsm.Autofighting ? Mathf.Min(SelfRole._inst.m_LockDis, StateInit.Instance.Distance) : SelfRole._inst.m_LockDis)
                && _off_dis < dis)
                {
                    t_dis = _off_dis;
                    t_role = m;
                }
            }
            if (m is MDC000 && ((MDC000)m).escort_name == A3_LegionModel.getInstance().myLegion.clname) continue;
            if (m is MDC000 && (int)(((float)((MDC000)m).curhp / (float)((MDC000)m).maxHp) * 100) <= 20) continue;
            if ((taskMonId?.applied ?? false) && taskMonId.value != m.monsterid) continue;
            if (handle?.Invoke(m) ?? false) continue;
            if (onTask
                && PlayerModel.getInstance().task_monsterIdOnAttack.ContainsKey(A3_TaskModel.getInstance().main_task_id)
                    && m.monsterid != PlayerModel.getInstance().task_monsterIdOnAttack[A3_TaskModel.getInstance().main_task_id])
                continue;
            if (TeamProxy.getInstance().MyTeamData != null)
            {
                if (A3_ActiveModel.getInstance().mwlr_on)
                    if (A3_ActiveModel.getInstance().mwlr_target_monId != m.monsterid)
                        continue;
                if (m.ownerName != null && !TeamProxy.getInstance().MyTeamData.IsInMyTeam(m.ownerName))
                    continue;
            }
            else if (m.ownerName != null && m.ownerName != PlayerModel.getInstance().name) continue;
            if (pkState != PK_TYPE.PK_PKALL && m is MS0000 && ((MS0000)m).owner_cid != 0)
                continue;
            float off_dis = (m.m_curPhy.position - pos).magnitude;
            if (off_dis < (SelfRole.fsm.Autofighting ? Mathf.Min(SelfRole._inst.m_LockDis, StateInit.Instance.Distance) : SelfRole._inst.m_LockDis)
                && off_dis < dis)
            {
                dis = off_dis;
                role = m;
            }
        }
        if (role != null && useMark)
            role.m_isMarked = true;
        else if (role == null) role = t_role;
        return role;
    }
    /// <summary>
    /// 查找指定位置最近的召唤兽
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public MonsterRole FindNearestSummon(Vector3 pos)
    {
        MonsterRole role = null;
        float dis = float.MaxValue;
        foreach (MonsterRole m in m_mapMonster.Values)
        {
            int ownerCid = 0;
            if (m is MS0000)
                ownerCid = ((MS0000)m).owner_cid;
            if (m.m_isMarked) continue;
            if (ownerCid == 0) continue;
            else
            {
                if (PlayerModel.getInstance().pk_state != PK_TYPE.PK_PEACE)
                {
                    if (ownerCid == PlayerModel.getInstance().cid) continue;
                    else if (PlayerModel.getInstance().pk_state == PK_TYPE.PK_TEAM)
                    {
                        if (TeamProxy.getInstance().MyTeamData != null && (TeamProxy.getInstance().MyTeamData.itemTeamDataList?.Exists((member) => member.cid == ownerCid) ?? false))
                            continue;
                        else
                            if (ownerCid == PlayerModel.getInstance().cid) continue;
                    }
                }
            }

            float off_dis = (m.m_curPhy.position - pos).magnitude;
            if (off_dis < (SelfRole.fsm.Autofighting ? Mathf.Min(SelfRole._inst.m_LockDis, StateInit.Instance.Distance) : SelfRole._inst.m_LockDis)
                && off_dis < dis)
            {
                dis = off_dis;
                role = m;
            }
        }
        return role;
    }
    public MonsterRole FindNearestFakeMonster(Vector3 pos)
    {
        float dis = 9999999f;

        MonsterRole role = null;
        foreach (MonsterRole m in m_mapFakeMonster.Values)
        {
            if (m.isDead || m is CollectRole || (m is MS0000 && ((MS0000)m).owner_cid == PlayerModel.getInstance().cid)||
                (m is MDC000 && ((MDC000)m).escort_name == A3_LegionModel.getInstance().myLegion.clname)) continue;
            if (m is MDC000 && (int)(((float)((MDC000)m).curhp / (float)((MDC000)m).maxHp) * 100) <= 20) continue;
            Vector3 off_pos = m.m_curPhy.position - pos;
            float off_dis = off_pos.magnitude;
            if (off_dis < SelfRole._inst.m_LockDis && off_dis < dis)
            {
                //debug.Log("遍历怪物位置");
                dis = off_dis;
                role = m;
            }
        }

        return role;
    }
    
    public Vector3 FindNearestMonsterPos(Vector3 me)
    {
        MonsterRole role = FindNearestMonster(me);
        if (role != null)
            return role.m_curPhy.position;
        return Vector3.zero;
    }

    public void clear()
    {
        foreach (MonsterRole m in m_mapMonster.Values)
        {
            m.dispose();
            //if (m is MonsterPlayer)
            //{
            //    m.dispose();
            //}
        }

        foreach (MonsterRole m in m_mapFakeMonster.Values)
        {
            m.dispose();
        }
        //foreach (MonsterPlayer role in m_mapMonster.Values)
        //{
        //    role.dispose();
        //}
        m_mapMonster.Clear();
        m_mapFakeMonster.Clear();
        m_listMonster.Clear();
        roleSummonMapping.Clear();
    }

    private List<MonsterRole> need_remove_list = new List<MonsterRole>();
    public void FrameMove(float fdt)
    {
        foreach (MonsterRole m in m_mapMonster.Values)
        {
            m.FrameMove(fdt);
        }

        need_remove_list.Clear();
        foreach (MonsterRole m in m_mapFakeMonster.Values)
        {
            m.FrameMove(fdt);

            //移除单机模式下的怪物移除（溶解动画播放之后）
            if (m.m_remove_after_dead)
            {
                need_remove_list.Add(m);
            }
        }

        foreach (MonsterRole m in need_remove_list)
        {
            RemoveMonster(m);
        }
    }
    public float GetTraceRange(int mid)
    {
        if (dMon.ContainsKey(mid))
            return (dMon[mid]?.GetNode("ai")?.getFloat("tracerang") ?? 0) / GameConstant.PIXEL_TRANS_UNITYPOS;
        else
            return 0;
    }

}

public class MonEffData
{
    public string id;
    public string file;
    public float y;
    public float f;
    public string sound;
    public float speed;
    public bool romote;
    public bool Lockpos;
    public float rotation;
};

public class TaskMonId
{
    public int value;
    public bool applied; //可以用作缓存
    public static implicit operator int(TaskMonId taskMonId) => taskMonId.value;
    public static implicit operator TaskMonId(int val) => new TaskMonId { value = val, applied = true };
}
