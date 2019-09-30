using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using MuGame;
using Cross;
public class PlayerData
{
    public int roleId;
    //public int monid;
}

public class OtherPlayerMgr
{
    static public OtherPlayerMgr _inst = new OtherPlayerMgr();

    public Dictionary<uint, ProfessionRole> m_mapOtherPlayer = new Dictionary<uint, ProfessionRole>();
    public Dictionary<uint, ProfessionRole> m_mapOtherPlayerSee = new Dictionary<uint, ProfessionRole>();
    public Dictionary<uint, ProfessionRole> m_mapOtherPlayerUnsee = new Dictionary<uint, ProfessionRole>();

    public List<Variant> cacheProxy = new List<Variant>();

    //private int idIdx = 0;
    public void AddOtherPlayer(Variant p)
    {
        if (GRMap.grmap_loading)
        {
            cacheProxy.Add(p);
            return;
        }
        debug.Log(p["name"] +"--------------:" + p.dump());
        uint iid = p["iid"]._uint;
        uint lvlsideid = 0;
        if (p.ContainsKey ("lvlsideid")) {
            lvlsideid = p["lvlsideid"];
        }
        if (m_mapOtherPlayer.ContainsKey(iid))
            return;
        uint cid = 0;
        int type = p["carr"];
        if(p.ContainsKey("cid"))
        { cid = p["cid"]._uint; }
       
        string name = p["name"];
        Vector3 born_pt = new Vector3(p["x"] / GameConstant.PIXEL_TRANS_UNITYPOS, 0f, p["y"] / GameConstant.PIXEL_TRANS_UNITYPOS);

        ProfessionRole pro_role = null;

        bool invisible = false;

        if ( p.ContainsKey( "invisible" ) && p[ "invisible" ] > 0 )
        {
            invisible = p[ "invisible" ] > 0;
        }

        int title_sign=0;
        //if ( p.ContainsKey( "title_sign" ) )
        //{
        //    title_sign = p[ "title_sign" ]._int;
        //}
        if (2 == type)
        {
            pro_role = new P2Warrior();
            if (SceneCamera.m_nModelDetail_Level != 1)
                (pro_role as P2Warrior).Init(name, EnumLayer.LM_DEFAULT, born_pt, false, p );
            else
                (pro_role as P2Warrior).Init(name, invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_OTHERPLAYER , born_pt, false, p  );

        }
        else if (3 == type)
        {
            pro_role = new P3Mage();
            if (SceneCamera.m_nModelDetail_Level != 1)
                (pro_role as P3Mage).Init(name, EnumLayer.LM_DEFAULT, born_pt, false , p  );
            else
                (pro_role as P3Mage).Init(name, invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_OTHERPLAYER , born_pt, false , p  );

        }
        else if (5 == type)
        {
            pro_role = new P5Assassin();
            if (SceneCamera.m_nModelDetail_Level != 1)
                (pro_role as P5Assassin).Init(name, EnumLayer.LM_DEFAULT, born_pt, false , p  );
            else
                (pro_role as P5Assassin).Init(name, invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_OTHERPLAYER , born_pt, false , p  );
        }
        else
        {
            pro_role = new P3Mage();
            if (SceneCamera.m_nModelDetail_Level != 1)
                (pro_role as P3Mage).Init(name, EnumLayer.LM_DEFAULT, born_pt, false , p  );
            else
                (pro_role as P3Mage).Init(name, invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_OTHERPLAYER , born_pt, false , p  );
        }

        pro_role.m_layer = invisible ? EnumLayer.LM_ROLE_INVISIBLE : EnumLayer.LM_OTHERPLAYER ;

        if (GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3342] || GameRoomMgr.getInstance().curRoom == GameRoomMgr.getInstance().dRooms[3358])
        {//解决多层阻挡点寻路的问题
            pro_role.setNavLay(NavmeshUtils.allARE);
        }

        //暂时都是可以自由pk的
        pro_role.m_curModel.position = born_pt;
        if (p.ContainsKey("face"))
        {
            pro_role.m_curModel.rotation = new Quaternion(pro_role.m_curModel.rotation.x,p["face"], pro_role.m_curModel.rotation.z,0);
        }
        if (p.ContainsKey("lvlsideid"))
        {
            if (lvlsideid != 0 && PlayerModel.getInstance().inSpost)
            {
                if (1 == lvlsideid)
                {
                    pro_role.refresNameColor_spost(SPOSTNAME_TYPE.RED);
                } else if (2 == lvlsideid)
                    pro_role.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
            }
            else if (lvlsideid != 0 && PlayerModel.getInstance().inCityWar)
            {
                if (lvlsideid == PlayerModel.getInstance().lvlsideid)
                {
                    pro_role.refresNameColor_spost(SPOSTNAME_TYPE.BULE);
                }
                else {
                    pro_role.refresNameColor_spost(SPOSTNAME_TYPE.RED);
                }
                
            }

        }

        if (pro_role == null)
            return;

        m_mapOtherPlayer.Add(iid, pro_role);
        m_mapOtherPlayerUnsee.Add(iid, pro_role);
        if (GRMap.curSvrConf["id"] == 24)
        {
            pro_role.setPos(new Vector3(born_pt.x, born_pt.y, born_pt.z + 18f));
        }
        else {
            pro_role.setPos(born_pt);
        }
        pro_role.SetDestPos(born_pt);
        pro_role.m_unIID = iid;
        pro_role.m_unCID = cid;
        pro_role.m_unPK_Param = cid;
        pro_role.lvlsideid = lvlsideid;
        pro_role.invisible = invisible;

        pro_role.spost_lvlsideid = (int)lvlsideid;
        debug.Log("对方是"+ lvlsideid);

        pro_role.speed =p[ "speed" ]._int;

        pro_role.maxHp = p["battleAttrs"]["max_hp"];
        pro_role.curhp = p["hp"];
        if(p.ContainsKey ("zhuan"))
        pro_role.zhuan = p["zhuan"];
        if(p.ContainsKey("pk_state"))
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
            case 5:
                pro_role.m_ePK_Type = PK_TYPE.Pk_SPOET;
                break;
        };
        if (p.ContainsKey("teamid"))
        {
            pro_role.m_unTeamID = p["teamid"];
            PlayerNameUIMgr.getInstance().refeshHpColor( pro_role );
        }
        if (p.ContainsKey("clanid"))
        {
            pro_role.m_unLegionID = p["clanid"];
        }
        if (p.ContainsKey("pet"))
        {
            pro_role.ChangePetAvatar(p["pet"]["id"], 0);
        }
        if (p.ContainsKey("treasure_num"))
        {
            pro_role.mapCount = p["treasure_num"];
            debug.Log("对方宝图"+ p["treasure_num"]);
            pro_role.refreshmapCount(p["treasure_num"]);
        }
        if (p.ContainsKey ("serial_kp"))
        {
            pro_role.serial = p["serial_kp"];
            pro_role.refreshserialCount(p["serial_kp"]);
        }
        if (p.ContainsKey ("vip"))
        {
            pro_role.refreshVipLvl((uint)p["vip"]);
        }

        if(p.ContainsKey("strike_back_tm")&& p["strike_back_tm"]>0)
        {
            pro_role.havefanjibuff = true;
            //也会发78号，不做处理也显示正常zzz
            debug.Log("这个人身上有反击buff");
        }else
        {
            pro_role.havefanjibuff = false;
        }

        //if (GRMap.grmap_loading == false)
        //    pro_role.refreshViewType(1);

        //if (pro_role != null)
        //{
        //    PlayerData dta = new PlayerData();
        //    dta.roleId = iid;
        //    //dta.monid = type;
        //    pro_role.playerDta = dta;
        //}

        //idIdx++;

        //if (mon != null)
        //    dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_ADD, this, mon));
    }
    public void refreshPlayerInfo(Variant v ,bool b = true)
    {
        uint iid = v["iid"];
        if (m_mapOtherPlayer.ContainsKey(iid))
        {
            m_mapOtherPlayer[iid].refreshViewData(v);
            //这个人是否有反击buff，有的话和平模式是可以攻击的
            if ( b )
            {
                if ( v.ContainsKey( "strike_back_tm" )&& v[ "strike_back_tm" ]>0 )
                    m_mapOtherPlayer[ iid ].havefanjibuff = true;
                else
                    m_mapOtherPlayer[ iid ].havefanjibuff = false;
            }
           
        }
    }


    public ProfessionRole GetOtherPlayer(uint iid)
    {
        if (!m_mapOtherPlayer.ContainsKey(iid))
            return null;

        return m_mapOtherPlayer[iid];
    }

    public void clear()
    {
        foreach (ProfessionRole role in m_mapOtherPlayer.Values)
        {
            role.dispose();
        }

        m_mapOtherPlayer.Clear();
        m_mapOtherPlayerSee.Clear();
        m_mapOtherPlayerUnsee.Clear();
    }


    public void RemoveOtherPlayer(uint iid)
    {
        if (!m_mapOtherPlayer.ContainsKey(iid))
            return;

        ProfessionRole pr = m_mapOtherPlayer[iid];
        if (m_mapOtherPlayerUnsee.ContainsKey(iid))
            m_mapOtherPlayerUnsee.Remove(iid);
        if (m_mapOtherPlayerSee.ContainsKey(iid))
            m_mapOtherPlayerSee.Remove(iid);
        FriendProxy.getInstance().removeNearyByLeave(pr.m_unCID);

        m_mapOtherPlayer.Remove(iid);
        if (a3_liteMiniBaseMap.instance != null)
        {
            a3_liteMiniBaseMap.instance.removeRoleInMiniMap(pr.strIID);
        }
        pr.dispose();

        if (SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.m_unIID == iid)
            SelfRole._inst.m_LockRole = null;

        //dispatchEvent(GameEvent.Create(MonsterMgr.EVENT_MONSTER_REMOVED, this, d));
    }

    public ProfessionRole FindWhoPhy(Transform phy)
    {
        foreach (ProfessionRole p in m_mapOtherPlayer.Values)
        {
            if (p.isDead || p.invisible) continue;
            //if (p.m_eFight_Side != FIGHT_A3_SIDE.FA3S_ENEMYOTHER) continue;

            if (p.m_curPhy == phy)
            {
                return p;
            }
        }

        return null;
    }


    public void onMapLoaded()
    {
        //foreach (ProfessionRole role in m_mapOtherPlayer.Values)
        //{
        //    role.refreshViewType(1);
        //}


        foreach (Variant v in cacheProxy)
        {
            AddOtherPlayer(v);
        }
        cacheProxy.Clear();
    }
    /// <summary>
    /// 检索离指定位置最近的敌对玩家
    /// </summary>
    /// <param name="pos">指定位置</param>
    /// <param name="isredname">是否仅针对红名角色</param>
    /// <param name="selector">过滤器:跳过对符合过滤条件玩家的检测</param>
    /// <param name="useMark">是否禁用重复检索,建议与selector配合使用</param>
    /// <param name="pkState">指定PK状态</param>
    /// <returns></returns>
    public BaseRole FindNearestEnemyOne(Vector3 pos, bool isredname = false,Func<ProfessionRole,bool> selector=null,bool useMark=false, PK_TYPE pkState = PK_TYPE.PK_PEACE)
    {
        float dis = 9999999f;
        BaseRole near_one = null;
        Func<BaseRole, bool> filterHandle;
        switch (pkState)
        {
            default:
            case PK_TYPE.PK_PKALL:
            case PK_TYPE.PK_PEACE:
                filterHandle = (p) => !p.isDead && (p.m_curPhy.position - pos).magnitude < (SelfRole.fsm.Autofighting ? StateInit.Instance.Distance : SelfRole._inst.m_LockDis);
                break;
            case PK_TYPE.PK_TEAM:
                filterHandle = (p) => !p.isDead && (p.m_curPhy.position - pos).magnitude < (SelfRole.fsm.Autofighting ? StateInit.Instance.Distance : SelfRole._inst.m_LockDis)
                                && ((!TeamProxy.getInstance().MyTeamData?.itemTeamDataList?.Exists((m) => m.cid == p.m_unCID)) ?? true);
                break;

            case PK_TYPE.Pk_SPOET:
                filterHandle = (p) => !p.isDead && (p.m_curPhy.position - pos).magnitude < (SelfRole.fsm.Autofighting ? StateInit.Instance.Distance : SelfRole._inst.m_LockDis)
                                && ((PlayerModel .getInstance ().lvlsideid != p.lvlsideid));
                break;
        }
        bool remarkDone = false;
        SEARCH_AGAIN:
        foreach (ProfessionRole p in m_mapOtherPlayer.Values)
        {
            if (OtherPlayerMgr._inst.m_mapOtherPlayer[p.m_unIID].zhuan >= 1)
            {
                if (p.isDead || p.invisible)
                    continue;
                if (p.m_isMarked)
                    continue;
                //if (p.m_eFight_Side != FIGHT_A3_SIDE.FA3S_ENEMYOTHER) continue;                
                if (isredname)
                    if (p.rednm <= 0)
                        continue;
                if (selector?.Invoke(p) ?? false)
                    continue;
                //军团模式（队友和同军团忽略）
                if (pkState == PK_TYPE.PK_TEAM)
                {
                    if ((TeamProxy.getInstance().MyTeamData?.itemTeamDataList?.Exists((m) => m.cid == p.m_unCID)) ?? true)
                        continue;
                    if (PlayerModel.getInstance().clanid != 0 && PlayerModel.getInstance().clanid == p.m_unLegionID)
                        continue;
                } else if (pkState == PK_TYPE.PK_PEACE)
                {
                    if (!p.havefanjibuff)
                        continue;
                }
                else if (pkState == PK_TYPE.Pk_SPOET)
                {
                    if (p.lvlsideid == PlayerModel .getInstance ().lvlsideid) continue;
                }
                Vector3 off_pos = p.m_curModel.position - pos;
                float off_dis = off_pos.magnitude;
                if (off_dis < (SelfRole.fsm.Autofighting ? StateInit.Instance.Distance : SelfRole._inst.m_LockDis) && off_dis < dis)
                {
                    dis = off_dis;
                    near_one = p;
                }
            }
        }
        if (near_one == null)
        {         
            //near_one = MonsterMgr._inst.FindNearestSummon(pos);
            if (near_one == null)
            {
                RoleMgr.ClearMark(!useMark, pkState, filterHandle: filterHandle);
                if (!remarkDone)
                {
                    remarkDone = true;
                    goto SEARCH_AGAIN;
                }
            }
            else if(useMark)
                near_one.m_isMarked = true;
        }
        if (near_one != null && useMark)
            near_one.m_isMarked = true;
        
        return near_one;
    }



    public void FrameMove(float fdt)
    {
        foreach (ProfessionRole p in m_mapOtherPlayer.Values)
        {
            p.FrameMove(fdt);
        }

        if (VIEW_PLAYER_TYPE == 0)
            return;

        if (GRMap.grmap_loading)
            return;

        foreach (ProfessionRole role in m_mapOtherPlayerUnsee.Values)
        {
            m_mapOtherPlayerUnsee.Remove(role.m_unIID);
            m_mapOtherPlayerSee.Add(role.m_unIID, role);
            FriendProxy.getInstance().addNearyByPeople(role.m_unIID);

            PlayerInfoProxy.getInstance().sendLoadPlayerDetailInfo(role.m_unCID);


            return;
        }

    }

    int _VIEW_PLAYER_TYPE =1;//0不显示，1显示全部
    public int VIEW_PLAYER_TYPE
    {
        get
        {
            return _VIEW_PLAYER_TYPE;
        }
        set
        {
            if (_VIEW_PLAYER_TYPE == value)
                return;

            _VIEW_PLAYER_TYPE = value;
            if (_VIEW_PLAYER_TYPE == 0)
            {
                foreach (ProfessionRole role in m_mapOtherPlayerSee.Values)
                {
                    //if (GRMap.grmap_loading == false)
                    //    role.refreshViewType(1);
                    //else
                    //    role.refreshViewType(0);
                    m_mapOtherPlayerUnsee.Add(role.m_unIID, role);
                }
                m_mapOtherPlayerSee.Clear();
            }

        }
    }

    public void PlayPetAvatarChange(uint iid, int petid, int stage)
    {
        ProfessionRole role = null;
        m_mapOtherPlayer.TryGetValue(iid, out role);
        if (role == null)
            return;

        role.ChangePetAvatar(petid, 0);

        if (SceneCamera.m_nModelDetail_Level != 1)
        {
            if (role.m_myPetBird != null)
            {
                foreach (Transform tran in role.m_myPetBird.GetComponentsInChildren<Transform>())
                {
                    //debug.Log("改变了layer " + tran.name);
                    tran.gameObject.layer = EnumLayer.LM_DEFAULT;// 更改物体的Layer层
                }
            }
        }
    }
    public void otherPlayPetAvatarChange(uint iid, int petid, int stage)
    {
        ProfessionRole role = null;
        m_mapOtherPlayer.TryGetValue(iid, out role);
        if (role == null)
            return;
        role.ChangePetAvatar(petid, 0);

        if (SceneCamera.m_nModelDetail_Level != 1)
        {
            if (role.m_myPetBird != null)
            {
                foreach (Transform tran in role.m_myPetBird.GetComponentsInChildren<Transform>())
                {
                    //debug.Log("改变了layer " + tran.name);
                    tran.gameObject.layer = EnumLayer.LM_DEFAULT;// 更改物体的Layer层
                }
            }
        }
    }

    public void playerMapCountChange(uint iid, int count) {
        ProfessionRole role = null;
        m_mapOtherPlayer.TryGetValue(iid, out role);
        if (role == null)
            return;
        role.mapCount = count;
        debug.Log("对方宝图"+ count+"个");
        role.refreshmapCount(count);
    }

    public void otherserial_pk(uint iid, int count)
    {
        ProfessionRole role = null;
        m_mapOtherPlayer.TryGetValue(iid, out role);
        if (role == null)
            return;
        role.serial = count;
        role.refreshserialCount(count);
    }

    public void refreshOtherModel()
    {//隐藏和显示其他玩家模型
        if (SceneCamera.m_nModelDetail_Level == 1)
        {
            foreach (ProfessionRole role in m_mapOtherPlayerSee.Values)
            {
                if ( role.invisible == true )
                {
                    continue;  //隐身状态
                }
                //角色模型
                foreach (Transform tran in role.m_curGameObj.GetComponentsInChildren<Transform>())
                {
                    //debug.Log("改变了layer " + tran.name);
                    tran.gameObject.layer = EnumLayer.LM_OTHERPLAYER;// 更改物体的Layer层
                }
                try
                {
                    role.m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
                }
                catch (System.Exception ex)
                {

                }

                //宠物模型
                if (role.m_myPetBird != null)
                {
                    foreach (Transform tran in role.m_myPetBird.GetComponentsInChildren<Transform>())
                    {
                        //debug.Log("改变了layer " + tran.name);
                        tran.gameObject.layer = EnumLayer.LM_MONSTER;// 更改物体的Layer层
                    }
                }

                //装备特效
                role.show_equip_eff();
            }

            //召唤兽处理
            foreach(MonsterRole mon in MonsterMgr._inst.m_mapMonster.Values)
            { 
                //if ( mon is MS0000 &&  (mon as MS0000).invisibleState == true )
                //{
                //    continue;  //隐身状态
                //}

                if (mon.issummon && mon.masterid != PlayerModel.getInstance().cid)
                {
                    foreach (Transform tran in mon.m_curGameObj.GetComponentsInChildren<Transform>())
                    {
                        //debug.Log("改变了layer " + tran.name);
                        tran.gameObject.layer = EnumLayer.LM_MONSTER;// 更改物体的Layer层
                    }
                }
            }
        }
        else
        {
            foreach (ProfessionRole role in m_mapOtherPlayerSee.Values)
            {
                if ( role.invisible == true )
                {
                    continue;  //隐身状态

                }

                //角色模型
                foreach (Transform tran in role.m_curGameObj.GetComponentsInChildren<Transform>())
                {
                    //debug.Log("改变了layer " + tran.name);
                    tran.gameObject.layer = EnumLayer.LM_DEFAULT;// 更改物体的Layer层
                }

                //宠物模型
                if (role.m_myPetBird != null)
                {
                    foreach (Transform tran in role.m_myPetBird.GetComponentsInChildren<Transform>())
                    {
                        //debug.Log("改变了layer " + tran.name);
                        tran.gameObject.layer = EnumLayer.LM_DEFAULT;// 更改物体的Layer层
                    }
                }

                //装备特效
                role.hide_equip_eff();
            }

            //召唤兽处理
            foreach (MonsterRole mon in MonsterMgr._inst.m_mapMonster.Values)
            {

                //if ( mon is MS0000 &&  ( mon as MS0000 ).invisibleState == true )
                //{
                //    continue;  //隐身状态
                //}

                if ( mon.issummon && mon.masterid != PlayerModel.getInstance().cid)
                {
                    foreach (Transform tran in mon.m_curGameObj.GetComponentsInChildren<Transform>())
                    {
                        //debug.Log("改变了layer " + tran.name);
                        tran.gameObject.layer = EnumLayer.LM_DEFAULT;// 更改物体的Layer层
                    }
                }
            }
        }
    }
}
