using System;
using UnityEngine;
using System.Collections;
using MuGame.role;
using MuGame;
using Cross;
using GameFramework;

public enum PLAYER_AI_TYPE
{
    PAIT_NONE = 0,
    PAIT_IDLE = 1,
    PAIT_MOVE = 2,
    PAIT_ATTACK = 3,
}

public enum A3_PROFESSION
{
    None = 0,
    //common 1  //1预留给通用职业
    Warrior = 2,
    Mage = 3,
    Archer = 4,
    Assassin = 5, //刺客
    Knight = 6, //骑士
}
public enum HIDE_TYPE
{
    ALL = 0,
    SKILL = 1,
}

public class ProfessionRole : BaseRole
{
    static public GameObject ROLE_LVUP_FX;

    public float AttackCount
    {
        get
        {
            return m_fAttackCount;
        }
        set
        {
            m_fAttackCount = value;
            if (m_fAttackCount <= 0)
                m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 1e-5f, height: 1e-5f);
        }
    }
    public int zhuan = 0;//几转
    public int Pk_state;//攻击类型
    public bool m_bUserSelf = false; //用户自己
    public bool m_invisiable = false;
    public int lvl = 0;
    public int combpt = 0;
    public int mapId => GRMap.instance.m_nCurMapID;
    public string clanName;//骑士团名称
    public int m_nKeepSkillCount = 0; //蓄力类技能

    public PlayerData playerDta;
    protected PLAYER_AI_TYPE m_eThinkType = PLAYER_AI_TYPE.PAIT_IDLE;

    public A3_PROFESSION m_ePRProfession = A3_PROFESSION.None;
    public string m_strAvatarPath;
    public string m_strEquipEffPath;

    //private float m_fJumpCount;

    private float m_fWingTime = 0f;

    private int m_petID = -1;
    private int m_petStage = -1;
    public PetBird m_myPetBird = null;

    public int mapCount = 0;
    public int serial = 0;//连杀

    protected ProfessionAvatar m_proAvatar = new ProfessionAvatar();
    static public bool ismyself = false;//是不是玩家自己（称号）    

    public float baseoffest = 0f;
    public bool isUp = false;
    private int lastRideId = -1;
    private float phyY = 0f;

    public int dianjiTime = -1;
    public int _speed = 0;
    public int speed {
        set {

            _speed = value;

            m_proAvatar.speed = _speed;

        }
        get {
            return _speed;
        }
    }

    GameObject shadowgo;

    public float readRideTime = 0f;
    public bool compelet = false;
    public ProfessionAvatar GetAvatar { get { return m_proAvatar; } }

    public RoleStateHelper m_rshelper;
    public REDNAME_TYPE NameType { set; get; } = REDNAME_TYPE.RNT_NORMAL;

    public static bool isShowyingzi = false;

    public static Action<ProfessionRole> systemCallBack = null;

    public void Init(string prefab_path, int layer, Vector3 pos, bool isUser = false, Variant serverData = null, uint cid = 0)
    {
        m_strModelPath = prefab_path;
        m_layer = layer;
        m_roleDta.pos = pos;
        m_roleDta.rotate = 0;
        m_isMain = isUser;
        if (serverData != null)
        {
            m_unCID = serverData["cid"];
            if (serverData.ContainsKey("title_sign"))
                heroTitleID = serverData["title_sign"];
            if (serverData.ContainsKey("title_sign_display"))
                heroTitle_isShow = serverData["title_sign_display"];



        }


        //角色的资源动作是马上加载出来的
        GameObject obj_prefab = GAMEAPI.ABModel_LoadNow_GameObject(m_strModelPath);
        if (obj_prefab == null || obj_prefab == U3DAPI.DEF_GAMEOBJ)
        {
            Debug.LogError("not find model = " + m_strModelPath);
            obj_prefab = U3DAPI.U3DResLoad<GameObject>("def_role");
        }

        m_curGameObj = GameObject.Instantiate(obj_prefab) as GameObject;

        foreach (Transform tran in m_curGameObj.GetComponentsInChildren<Transform>())
        {
            //debug.Log("改变了layer " + tran.name);
            tran.gameObject.layer = m_layer;// 更改物体的Layer层
        }

        m_curModel = m_curGameObj.transform.FindChild("model");
        m_curPhy = m_curModel.transform.FindChild("physics");
        phyY = m_curPhy.localPosition.y;
        try
        {
            m_curPhy.gameObject.layer = EnumLayer.LM_BT_FIGHT;
        }
        catch (System.Exception ex)
        {

        }

        m_curAni = m_curModel.GetComponent<Animator>();

        CapsuleCollider CapsuleCollider = m_curPhy.GetComponent<CapsuleCollider>();
        headOffset = CapsuleCollider.center;
        headOffset.y += CapsuleCollider.height / 2;
        headOffset_half = CapsuleCollider.center;
        //4足位置（人左右手+左右脚  牛 4四只脚）
        m_LeftHand = m_curModel.FindChild("L_Finger1");
        m_RightHand = m_curModel.FindChild("R_Finger1");
        m_LeftFoot = m_curModel.FindChild("L_Toe0");
        m_RightFoot = m_curModel.FindChild("R_Toe0");


        if (m_LeftHand == null) m_LeftHand = U3DAPI.DEF_TRANSFORM;
        if (m_RightHand == null) m_RightHand = U3DAPI.DEF_TRANSFORM;
        if (m_LeftFoot == null) m_LeftFoot = U3DAPI.DEF_TRANSFORM;
        if (m_RightFoot == null) m_RightFoot = U3DAPI.DEF_TRANSFORM;

        //refreshViewType(viewType);
        //如断头 断尾要加效果的话要特殊处理

        //SelfHitPoint shp = m_curModel.gameObject.AddComponent<SelfHitPoint>();
        //shp.m_selfRole = this;

        //m_curPhy = m_curModel.FindChild("physics");
        //HurtPoint htt = m_curPhy.gameObject.AddComponent<HurtPoint>();
        //htt.m_selfRole = this;

        //PetBirdMgr.Inst.AttachPetBird(m_curModel);

        if (m_layer != EnumLayer.LM_SELFROLE && !(this is CollectRole) && TEMP_SHADOW != null)
        {
            shadowgo = GameObject.Instantiate(TEMP_SHADOW) as GameObject;
            shadowgo.transform.SetParent(m_curModel.transform, false);
            //当模型是有旋转时，调整影子的旋转反向
            Quaternion q = m_curModel.transform.localRotation;
            shadowgo.transform.localRotation = Quaternion.Inverse(q);

        }

        else if (this.m_isMain && TEMP_SHADOW != null) {

            shadowgo = GameObject.Instantiate(TEMP_SHADOW) as GameObject;
            shadowgo.transform.SetParent(m_curModel.transform, false);
            //当模型是有旋转时，调整影子的旋转反向
            Quaternion q = m_curModel.transform.localRotation;
            shadowgo.transform.localRotation = Quaternion.Inverse(q);
            shadowgo.gameObject.SetActive(!isShowyingzi);

            systemCallBack = (selfpro) => {

                if (selfpro == null || selfpro.shadowgo == null)
                {
                    return;

                }

                selfpro.shadowgo.gameObject.SetActive(!isShowyingzi);

            };

        }

        m_rshelper = new RoleStateHelper(this);
        refreshViewType(999);

        m_unLegionID = (uint)LOGION_DEF.LNDF_PLAYER;

        if (m_bUserSelf)
        {
            SelfHurtPoint shtt = m_curPhy.gameObject.AddComponent<SelfHurtPoint>();
            shtt.m_selfRole = this;
            ismyself = true;
        }
        else
        {
            OtherHurtPoint ohtt = m_curPhy.gameObject.AddComponent<OtherHurtPoint>();
            ohtt.m_otherRole = this;
            ismyself = false;
        }


        //roleName = "我是主角";
        PlayerNameUIMgr.getInstance().show(this);

        curhp = maxHp = 2000;

        m_proAvatar.Init_PA(m_ePRProfession, m_strAvatarPath, "l_", m_curGameObj.layer, EnumMaterial.EMT_EQUIP_L, m_curModel, m_strEquipEffPath);

        //if (m_isMain && viewType != VIEW_TYPE_ALL)
        //    refreshViewType(VIEW_TYPE_ALL);

        if (m_isMain)
        {
            this.mapCount = (int)PlayerModel.getInstance().treasure_num;
            refreshmapCount((int)PlayerModel.getInstance().treasure_num);
            this.serial = PlayerModel.getInstance().serial;
            refreshserialCount(PlayerModel.getInstance().serial);
            refreshVipLvl((uint)A3_VipModel.getInstance().Level);
            PlayerNameUIMgr.getInstance().refeshHpColor(this);
        }

        if (m_moveAgent != null)
            m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 1e-5f, height: 1e-5f);
    }


    //
    public Renderer petRendrer = null;
    public Material petMaterialOri = null;

    public void ChangePetAvatar(int petID, int petStage)
    {
        if (petID == m_petID && petStage == m_petStage)
            return;

        m_petID = petID;
        m_petStage = petStage;

        if (m_myPetBird != null)
        {
            GameObject.Destroy(m_myPetBird.gameObject);
            petRendrer = null;
            petMaterialOri = null;
            m_myPetBird = null;
        }

        Transform stop = m_curModel.FindChild("birdstop");
        string petava = A3_PetModel.getInstance().GetPetAvatar(m_petID, 0);
        if (petava == "")
            return;
        GameObject birdPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_" + petava);


        GameObject pathPrefab = GAMEAPI.ABModel_LoadNow_GameObject("profession_birdpath");
        if (birdPrefab == null || pathPrefab == null)
            return;

        GameObject bird = GameObject.Instantiate(birdPrefab, stop.position, Quaternion.identity) as GameObject;
        GameObject path = GameObject.Instantiate(pathPrefab, stop.position, Quaternion.identity) as GameObject;
        if (bird == null || path == null)
            return;

        path.transform.parent = stop;
        bird.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        m_myPetBird = bird.AddComponent<PetBird>();
        m_myPetBird.Path = path;

        petRendrer = m_myPetBird.GetComponentInChildren<SkinnedMeshRenderer>();

        if (petRendrer != null) petMaterialOri = petRendrer.material;

        if (invisible && m_myPetBird)
        {
            foreach (Transform tran in m_myPetBird.gameObject.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;

            }
        }

        if (m_isMain)
        {
            ChangePetInvisible(invisible);
        }

    }

    public void ChangePetInvisible(bool isinvisibl) {

        if (petRendrer == null)
        {
            return;
        }

        if (isinvisibl)

        {
            petRendrer.material = EnumMaterial.EMT_SKILL_HIDE;

        }

        else
        {
            petRendrer.material = petMaterialOri;

        }

    }

    protected override void onRefresh_ViewType()
    {
        if (!m_isMain && m_moveAgent != null && m_moveAgent.enabled)
            m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 1e-5f, height: 1e-5f);


        ////modify
        //if (viewType == BaseRole.VIEW_TYPE_ALL)
        //{
        //    set_weaponl(m_roleDta.m_Weapon_LID, m_roleDta.m_Weapon_LFXID); //weapon_l = m_roleDta.m_Weapon_LID;
        //    set_weaponr(m_roleDta.m_Weapon_RID, m_roleDta.m_Weapon_RFXID); //weapon_r = m_roleDta.m_Weapon_RID;
        //    set_wing(m_roleDta.m_WindID, m_roleDta.m_WingFXID);// wing = m_roleDta.m_WindID;
        //    set_body(m_roleDta.m_BodyID, m_roleDta.m_BodyFXID); //body = m_roleDta.m_BodyID;

        //    //添加动画重绑
        //    rebind_ani();
        //    set_equip_color(m_roleDta.m_EquipColorID);
        //    if (isDead)
        //        onDead(true);
        //    //修改只在玩家切地图的时候播光，普通时候不播
        //    //else
        //    //{
        //    //    GameObject goeff = GameObject.Instantiate(worldmap.EFFECT_CHUANSONG2) as GameObject;
        //    //    goeff.transform.SetParent(m_curModel, false);
        //    //    GameObject.Destroy(goeff, 2f);
        //    //}

        //}
        //else if (viewType == BaseRole.VIEW_TYPE_NAV)
        //{


        //    m_proAvatar.set_weaponl(-1, 0);
        //    m_proAvatar.set_weaponr(-1, 0);
        //    m_proAvatar.set_wing(-1, 0);
        //    m_proAvatar.set_body(-1, 0);
        //    m_proAvatar.set_equip_color(m_roleDta.m_EquipColorID);
        //}
    }

    public Variant detailInfo;
    public RIDESTATE ridestate = RIDESTATE.Down;
    public int rideId = 0;
    public override void refreshViewData(Variant v)
    {
        if (!m_isMain && m_moveAgent != null && m_moveAgent.enabled)
            m_moveAgent.avoidancePriority = 48;
        detailInfo = v;
        int carr = v["carr"];



        if (v.ContainsKey("activate_count"))
        {
            m_roleDta.activecount = v["activate_count"];
            m_roleDta.add_eff = false;
            if (v["activate_count"] >= 10)
            {
                m_roleDta.add_eff = true;
            }
        }
        //装备
        if (v.ContainsKey("show_eqp"))
        {
            m_roleDta.m_BodyID = 0;
            m_roleDta.m_BodyFXID = 0;
            m_roleDta.m_EquipColorID = 0;
            m_roleDta.m_Weapon_LID = 0;
            m_roleDta.m_Weapon_LFXID = 0;
            m_roleDta.m_Weapon_RID = 0;
            m_roleDta.m_Weapon_RFXID = 0;

            int temporary_weaponid = 0;
            int temporary_weaponFxid = 0;
            if (v["show_eqp"]._arr != null && v["show_eqp"]._arr.Count > 0)
            {
                foreach (Variant p in v["show_eqp"]._arr)
                {
                    a3_ItemData data = a3_BagModel.getInstance().getItemDataById(p["tpid"]);
                    if (data.equip_type == 3 /*|| data.equip_type == 11*/)
                    {
                        int bodyid = (int)data.tpid;
                        int bodyFxid = p["stage"];
                        m_roleDta.m_BodyID = data.equip_type == 3 && m_roleDta.m_BodyID != 0 ? m_roleDta.m_BodyID : bodyid;
                        m_roleDta.m_BodyFXID = data.equip_type == 3 && m_roleDta.m_BodyFXID != 0 ? m_roleDta.m_BodyFXID : bodyFxid;
                        uint colorid = 0;
                        if (p.ContainsKey("colour"))
                            colorid = p["colour"];
                        m_roleDta.m_EquipColorID = colorid;
                    }


                    if (data.equip_type == 6 || data.equip_type == 12)
                    {
                        temporary_weaponid = data.equip_type == 6 && temporary_weaponid != 0 ? temporary_weaponid : (int)data.tpid;
                        int stage = p["stage"];
                        temporary_weaponFxid = data.equip_type == 6 && temporary_weaponFxid != 0 ? temporary_weaponFxid : stage;
                        int weaponid = temporary_weaponid;
                        int weaponFxid = temporary_weaponFxid;

                        switch (carr)
                        {
                            case 2:
                                m_roleDta.m_Weapon_RID = weaponid;
                                m_roleDta.m_Weapon_RFXID = weaponFxid;
                                break;
                            case 3:
                                m_roleDta.m_Weapon_LID = weaponid;
                                m_roleDta.m_Weapon_LFXID = weaponFxid;
                                break;
                            case 5:
                                m_roleDta.m_Weapon_LID = weaponid;
                                m_roleDta.m_Weapon_LFXID = weaponFxid;
                                m_roleDta.m_Weapon_RID = weaponid;
                                m_roleDta.m_Weapon_RFXID = weaponFxid;
                                break;
                        }
                    }
                }
            }
            else
            {
                //有时装没装备
                if (A3_FashionShowModel.getInstance().first_nowfs[1] != 0)
                    set_body(A3_FashionShowModel.getInstance().first_nowfs[1], 0);
                else
                    set_body(0, 0);
                if (A3_FashionShowModel.getInstance().first_nowfs[0] != 0)
                {
                    set_weaponr(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                    if (PlayerModel.getInstance().profession == 5)
                        set_weaponl(A3_FashionShowModel.getInstance().first_nowfs[0], 0);
                }
                else
                {
                    set_weaponr(0, 0);
                    if (PlayerModel.getInstance().profession == 5)
                        set_weaponl(0, 0);


                }
            }
        }

        clear_eff();
        if (m_roleDta.add_eff)
        {
            set_equip_eff(m_roleDta.m_BodyID, false);
        }
        set_equip_eff(a3_EquipModel.getInstance().GetEff_lvl(m_roleDta.activecount));
        if (!m_isMain && SceneCamera.m_nModelDetail_Level != 1)
        {
            hide_equip_eff();
        }

        //飞翼
        if (v.ContainsKey("wing"))
        {
            m_roleDta.m_WindID = v["wing"]["show_stage"];
        }
        //军衔SS
        if (v.ContainsKey("ach_title"))
        {
            title_id = v["ach_title"];
            isactive = v["title_display"]._bool;
            PlayerNameUIMgr.getInstance().refreshTitlelv(this, title_id);
        }

        debug.Log("这这这" + v.dump());

        if (v.ContainsKey("serial_kp"))
        {
            PlayerNameUIMgr.getInstance().refreserialCount(this, v["serial_kp"]);
        }
        //红名类型：
        if (v.ContainsKey("rednm"))
        {
            rednm = v["rednm"];
            PlayerNameUIMgr.getInstance().refreshNameColor(this, rednm);
            //点击的角色在红名类型变更了
            if (SelfRole._inst != null && SelfRole._inst.m_LockRole != null && SelfRole._inst.m_LockRole.m_unIID == v["iid"])
            {
                PkmodelAdmin.RefreshShow(SelfRole._inst.m_LockRole, false, true);
            }
        }
        //反击buff时间戳
        if (v.ContainsKey("strike_back_tm"))
        {
            hidbacktime = v["strike_back_tm"];
            PlayerNameUIMgr.getInstance().refresHitback(this, (int)(hidbacktime - (uint)NetClient.instance.CurServerTimeStamp));
            //debug.Log("时间1：" + hidbacktime + "时间2：" + (uint)NetClient.instance.CurServerTimeStamp);
        }


        if (v.ContainsKey("lvl"))
            lvl = v["lvl"];

        if (v.ContainsKey("combpt"))
        {
            combpt = v["combpt"];
        }
        if (v.ContainsKey("clname"))
        {
            clanName = v["clname"];
        }
        ArrayList arry = new ArrayList();
        arry.Add(m_unCID);
        arry.Add(combpt);
        if (FriendProxy.getInstance() != null)
            FriendProxy.getInstance().reFreshProfessionInfo(arry);

        //if (OtherPlayerMgr._inst.VIEW_PLAYER_TYPE == 1 || m_isMain)
        //    refreshViewType(VIEW_TYPE_ALL);
        //  base.refreshViewData(v);


        int wqponl_id = 0;
        int wqponr_id = 0;
        int wqponl_lfxid = 0;
        int wqponr_rfxid = 0;
        int body_id = 0;
        int body_dxid = 0;
        //时装
        if (v.ContainsKey("dress_list"))
        {
            if(v["dress_list"]!=null&&v["dress_list"].Count>0)
            {
                wqponl_id = wqponr_id = v["dress_list"][0];
                body_id = v["dress_list"][1];
            }
            else
            {
                body_id = m_roleDta.m_BodyID;
                body_dxid = m_roleDta.m_BodyFXID;
                wqponr_id = m_roleDta.m_Weapon_LID;
                wqponr_id = m_roleDta.m_Weapon_RID;
                wqponl_lfxid = m_roleDta.m_Weapon_LFXID;
                wqponr_rfxid = m_roleDta.m_Weapon_RFXID;
            }
        }

        set_weaponl(wqponl_id, wqponl_lfxid);
        set_weaponr(wqponr_id, wqponr_rfxid);
        set_wing(m_roleDta.m_WindID, m_roleDta.m_WingFXID);
        set_body(body_id, body_dxid);

        //if ( GRMap.curSvrConf != null && GRMap.curSvrConf.ContainsKey( "maptype" ) && GRMap.curSvrConf[ "maptype" ]._int > 0 )
        //{
        //        //副本中不显示坐骑
        //}
        //else {

        //坐骑

        if (v.ContainsKey("ride_info"))
        {
            rideId = v["ride_info"]["dress"]._int;
        }

        if (v.ContainsKey("ride_info") && v["ride_info"]["mount"]._uint == (uint)RIDESTATE.UP)
        {
            ridestate = RIDESTATE.UP;

            ChangeRideState(true);
        }
        else if (v.ContainsKey("ride_info") && v["ride_info"]["mount"]._uint == (uint)RIDESTATE.Down)
        {
            ridestate = RIDESTATE.Down;

            Remove_Ride();
        }


        //}

    }


    //public void refreshViewData1(Variant v)
    //{
    //    int carr = v["carr"];
    //    if (v.ContainsKey("eqp"))
    //    {
    //        m_roleDta.m_BodyID = 0;
    //        m_roleDta.m_BodyFXID = 0;
    //        m_roleDta.m_EquipColorID = 0;
    //        m_roleDta.m_Weapon_LID = 0;
    //        m_roleDta.m_Weapon_LFXID = 0;
    //        m_roleDta.m_Weapon_RID = 0;
    //        m_roleDta.m_Weapon_RFXID = 0;

    //        foreach (Variant p in v["eqp"]._arr)
    //        {
    //            a3_ItemData data = a3_BagModel.getInstance().getItemDataById(p["tpid"]);
    //            if (data.equip_type == 3)
    //            {
    //                int bodyid = (int)data.tpid;
    //                int bodyFxid = p["intensify"];
    //                m_roleDta.m_BodyID = bodyid;
    //                m_roleDta.m_BodyFXID = bodyFxid;

    //                uint colorid = 0;
    //                if (p.ContainsKey("colour"))
    //                    colorid = p["colour"];
    //                m_roleDta.m_EquipColorID = colorid;
    //            }
    //            if (data.equip_type == 6)
    //            {
    //                int weaponid = (int)data.tpid;
    //                int weaponFxid = p["intensify"];
    //                switch (carr)
    //                {
    //                    case 2:
    //                        m_roleDta.m_Weapon_RID = weaponid;
    //                        m_roleDta.m_Weapon_RFXID = weaponFxid;
    //                        break;
    //                    case 3:
    //                        m_roleDta.m_Weapon_LID = weaponid;
    //                        m_roleDta.m_Weapon_LFXID = weaponFxid;
    //                        break;
    //                    case 5:
    //                        m_roleDta.m_Weapon_LID = weaponid;
    //                        m_roleDta.m_Weapon_LFXID = weaponFxid;
    //                        m_roleDta.m_Weapon_RID = weaponid;
    //                        m_roleDta.m_Weapon_RFXID = weaponFxid;
    //                        break;
    //                }
    //            }
    //        }
    //    }
    //    if (v.ContainsKey("wing"))
    //    {
    //        m_roleDta.m_WindID = v["wing"];
    //    }
    //    //军衔SS
    //    if (v.ContainsKey("ach_title"))
    //    {
    //        title_id = v["ach_title"];
    //        isactive = v["title_display"]._bool;
    //        PlayerNameUIMgr.getInstance().refreshTitlelv(this, title_id);
    //    }

    //    if (v.ContainsKey("lvl"))
    //        lvl = v["lvl"];

    //    if (v.ContainsKey("combpt"))
    //    {
    //        combpt = v["combpt"];
    //    }
    //    if (v.ContainsKey("clname"))
    //    {
    //        clanName = v["clname"];
    //    }
    //    ArrayList arry = new ArrayList();
    //    arry.Add(m_unCID);
    //    arry.Add(combpt);
    //    if (FriendProxy.getInstance() != null)
    //        FriendProxy.getInstance().reFreshProfessionInfo(arry);

    //    //if (OtherPlayerMgr._inst.VIEW_PLAYER_TYPE == 1 || m_isMain)
    //    //    refreshViewType(VIEW_TYPE_ALL);
    //}

    public override void dispose()
    {
        shadowgo = null;
        base.dispose();
        m_proAvatar.dispose();
        m_proAvatar = null;

        //GameObject.Destroy(m_curGameObj);
        if (m_myPetBird != null)
        {
            GameObject.Destroy(m_myPetBird.gameObject);
        }
    }

    public int get_weaponl_id()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_Weapon_LID;

        return m_proAvatar.m_Weapon_LID;
    }

    public int get_weaponl_fxid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_Weapon_LFXID;

        return m_proAvatar.m_Weapon_LFXLV;
    }

    public void set_weaponl(int id, int fxlevel)
    {
        if (viewType != VIEW_TYPE_ALL)
        {
            m_roleDta.m_Weapon_LID = id;
            m_roleDta.m_Weapon_LFXID = fxlevel;
            return;
        }

        if (m_proAvatar.weaponL_CallBack == null)
        {
            m_proAvatar.weaponL_CallBack = () => {

                if (invisible && m_isMain)
                {
                    m_proAvatar.push_fx(1, true);
                }
            };
        }

        m_proAvatar.set_weaponl(id, fxlevel);

        //if (invisible && m_isMain)
        //{
        //    m_proAvatar.push_fx(1, true);
        //}
    }

    public int get_weaponr_id()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_Weapon_RID;

        return m_proAvatar.m_Weapon_RID;
    }

    public int get_weaponr_fxid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_Weapon_RFXID;

        return m_proAvatar.m_Weapon_RFXLV;
    }

    public void set_weaponr(int id, int fxlevel)
    {
        if (viewType != VIEW_TYPE_ALL)
        {
            m_roleDta.m_Weapon_RID = id;
            m_roleDta.m_Weapon_RFXID = fxlevel;
            return;
        }

        if (m_proAvatar.weaponR_CallBack == null)
        {
            m_proAvatar.weaponR_CallBack = () => {

                if (invisible && m_isMain)
                {
                    m_proAvatar.push_fx(1, true);
                }
            };
        }


        m_proAvatar.set_weaponr(id, fxlevel);


        //if (invisible && m_isMain)
        //{
        //    m_proAvatar.push_fx(1, true);
        //}
    }

    public uint get_equip_colorid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_EquipColorID;

        return m_proAvatar.m_EquipColorID;
    }

    public bool invisible
    {
        set
        {
            m_invisiable = value;

            m_proAvatar.invisible = m_invisiable;

            if (m_isMain)
            {
                if (m_invisiable)
                {
                    m_proAvatar.push_fx(1);
                    m_bHide_state = true;
                    //  m_fHideTime = 6f;
                    ChangePetInvisible(true);
                }
                else
                {
                    LeaveHide();
                    ChangePetInvisible(false);
                }
            }
            else
            {
                if (!m_invisiable)
                {
                    ShowAll();
                }
                else
                {
                    HideAll();
                }
            }
        }

        get { return m_invisiable; }
    }

    public int get_wingid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_WindID;

        return m_proAvatar.m_WindID;// m_WindID;
    }

    public int get_windfxid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_WingFXID;

        return m_proAvatar.m_Wing_FXLV;// m_Wing_FXLV;
    }

    public void set_wing(int id, int fxlevel)
    {
        if (viewType != VIEW_TYPE_ALL)
        {
            m_roleDta.m_WindID = id;
            m_roleDta.m_WingFXID = fxlevel;
            return;
        }

        if (m_proAvatar.wing_CallBack == null)
        {
            m_proAvatar.wing_CallBack = () => {

                if (invisible && m_isMain)
                {
                    m_proAvatar.push_fx(1, true);
                }
            };
        }

        m_proAvatar.set_wing(id, fxlevel);

        if (m_proAvatar.rideGo != null)
        {
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 2f);
        }
        else if (m_proAvatar.m_WindID > 0 /*&& m_proAvatar.m_WingObj != null*/)
        {
            //Animation wing_anim = m_proAvatar.m_WingObj.GetComponent<Animation>();
            //if (wing_anim != null)
            //{
            //    m_Wing_Animstate = wing_anim["wg"];
            //}

            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 1f);
        }
        else
        {
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);
        }

        //if (invisible && m_isMain)
        //{
        //    m_proAvatar.push_fx(1, true);
        //}
    }


    public int get_bodyid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_BodyID;
        return m_proAvatar.m_BodyID;
    }

    public int get_bodyfxid()
    {
        if (viewType != VIEW_TYPE_ALL)
            return m_roleDta.m_BodyFXID;
        return m_proAvatar.m_BodyFXLV;
    }

    public void set_body(int id, int fxlevel)
    {
        if (viewType != VIEW_TYPE_ALL)
        {
            m_roleDta.m_BodyID = id;
            m_roleDta.m_BodyFXID = fxlevel;
            return;
        }

        if (m_proAvatar.bodyS_CallBack == null)
        {
            m_proAvatar.bodyS_CallBack = () => {

                if (invisible && m_isMain)
                {
                    m_proAvatar.push_fx(1, true);
                }
            };
        }

        if (m_proAvatar.body_CallBack == null)
        {
            m_proAvatar.body_CallBack = () => {

                if (invisible && m_isMain)
                {
                    m_proAvatar.push_fx(1, true);
                }
            };
        }

        m_proAvatar.set_body(id, fxlevel);

    }

    public void set_Ride(int id, bool isChangeMap = false)
    {

        if (id == -1 || lastRideId == id)
        {
            return;
        }

        lastRideId = id;

        ride_cd.show(() =>
        {

            if (m_proAvatar == null)
            {
                ride_cd.hide();

                return;
            }

            m_proAvatar.Set_Ride(id, (baseoffest) =>
            {

                if (m_moveAgent != null)
                {

                    baseoffest = (baseoffest == 0f && baseoffest == m_moveAgent.baseOffset) ? m_moveAgent.baseOffset : baseoffest;

                    m_moveAgent.baseOffset = baseoffest;

                    zuoji_Ani = m_proAvatar.rideGo.GetComponent<Animator>();

                    if (invisible && m_isMain)
                    {
                        m_proAvatar.push_fx(1, true);
                    }

                    m_curPhy.localPosition = new Vector3(m_curPhy.localPosition.x, phyY - baseoffest, m_curPhy.localPosition.z);

                }

                ////////////////

                if (m_isMain && A3_RideModel.getInstance().GetRideState() == (int)RIDESTATE.Down)
                {
                    this.Remove_Ride();
                }

                if (m_isMain == false && this.ridestate == RIDESTATE.Down)
                {
                    this.Remove_Ride();
                }

                if (A3_RideProxy.IsCanChangeRide(this.dianjiTime) == false)
                {
                    this.Remove_Ride();
                }

                m_proAvatar.SetShadowgoScale(id, shadowgo, baseoffest);

            });

        }, false);  //  需要读条 : isChangeMap ? false : m_isMain  

    }

    public void Remove_Ride()
    {
        m_proAvatar.remove_Ride();

        m_moveAgent.baseOffset = baseoffest;

        m_curPhy.localPosition = new Vector3(m_curPhy.localPosition.x, phyY, m_curPhy.localPosition.z);

        baseoffest = 0;

        zuoji_Ani = null;

        lastRideId = -1;

        this.m_curAni.speed = 1f;

        if (shadowgo != null)
        {
            shadowgo.transform.localPosition = Vector3.zero;
            shadowgo.transform.localScale = Vector3.one;
        }
    }

    public void clear_eff()
    {
        m_proAvatar.clear_oldeff();
    }

    public void set_equip_eff(int id, bool high)
    {
        m_proAvatar.set_equip_eff(id, high);
    }
    public void set_equip_eff(int lvl)
    {
        m_proAvatar.set_equip_eff(lvl);
    }

    public void show_equip_eff()
    {
        m_proAvatar.showEquipEff();
    }
    public void hide_equip_eff()
    {
        m_proAvatar.hideEquipEff();
    }

    public void set_equip_color(uint id)
    {
        if (viewType != VIEW_TYPE_ALL)
        {
            m_roleDta.m_EquipColorID = id;
            return;
        }

        m_proAvatar.set_equip_color(id);
    }

    public void FlyWing(float time)
    {
        if (m_proAvatar.m_Wing_Animstate != null)
        {
            m_proAvatar.m_Wing_Animstate.speed = 1.5f;
            m_fWingTime = time;
        }
    }

    public int getShowSkillEff()
    {
        if (SceneCamera.m_nSkillEff_Level > 1 && !m_isMain)
            return 2; //技能设置低档，只有动作
        else
            return 1;
    }

    public void rebind_ani()
    {
        m_curAni.Rebind();

        if (m_proAvatar.rideGo != null)
        {
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 2f);
        }

        else if (m_roleDta.m_WindID > 0)
        {
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 1f);
        }

        else
        {
            m_curAni.SetFloat(EnumAni.ANI_F_FLY, 0f);
        }
    }

    public HitData Link_PRBullet(uint skillid, float t, GameObject root, Transform linker)
    {
        HitData hd = linker.gameObject.AddComponent<HitData>();

        hd.m_hdRootObj = root;
        hd.m_CastRole = this;
        hd.m_vBornerPos = m_curModel.position;

        hd.m_ePK_Type = m_ePK_Type;
        switch (m_ePK_Type)
        {
            case PK_TYPE.PK_TEAM: hd.m_unPK_Param = m_unTeamID; break;
            case PK_TYPE.PK_LEGION: hd.m_unPK_Param = m_unLegionID; break;
            case PK_TYPE.PK_PKALL: hd.m_unPK_Param = m_unCID; break;
            case PK_TYPE.Pk_SPOET: hd.m_unPK_Param = lvlsideid; ; break;
        }

        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        linker.gameObject.layer = EnumLayer.LM_BT_FIGHT;
        GameObject.Destroy(root, t);

        //可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }
    public HitData Build_PRBullet(uint skillid, float t, GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject bult = GameObject.Instantiate(original, position, rotation) as GameObject;
        bult.transform.SetParent(U3DAPI.FX_POOL_TF, false);
        HitData hd = bult.gameObject.AddComponent<HitData>();

        hd.m_hdRootObj = bult;
        hd.m_CastRole = this;
        hd.m_vBornerPos = m_curModel.position;

        hd.m_ePK_Type = m_ePK_Type;
        switch (m_ePK_Type)
        {
            case PK_TYPE.PK_TEAM: hd.m_unPK_Param = m_unTeamID; break;
            case PK_TYPE.PK_LEGION: hd.m_unPK_Param = m_unLegionID; break;
            case PK_TYPE.PK_PKALL: hd.m_unPK_Param = m_unCID; break;
            case PK_TYPE.Pk_SPOET: hd.m_unPK_Param = lvlsideid; break;
        }

        hd.m_unSkillID = skillid;
        hd.m_nDamage = 100;
        hd.m_nHitType = 0;

        bult.layer = EnumLayer.LM_BT_FIGHT;

        if (t > 0f)
            GameObject.Destroy(bult, t);

        //可以用一个float 来记录最后一个子弹的时间，以后释放role的时候，可以通过这个考虑延时
        if (m_fDisposeTime < t) m_fDisposeTime = t;

        return hd;
    }


    public void LeaveHide()
    {
        if (m_bHide_state)
        {
            m_bHide_state = false;
            m_proAvatar.pop_fx();
            m_fHideTime = 0f;
        }
    }

    public void HideAll()
    {
        m_layer = EnumLayer.LM_ROLE_INVISIBLE;
        m_proAvatar.m_nLayer = EnumLayer.LM_ROLE_INVISIBLE;

        PlayerNameUIMgr.getInstance().hide(this);

        foreach (Transform tran in m_curGameObj.GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
        }

        if (m_myPetBird)
        {
            foreach (Transform tran in m_myPetBird.gameObject.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_ROLE_INVISIBLE;
            }
        }

    }


    public void ShowAll()
    {

        m_layer = m_isMain ? EnumLayer.LM_SELFROLE : EnumLayer.LM_OTHERPLAYER;

        m_proAvatar.m_nLayer = m_isMain ? EnumLayer.LM_SELFROLE : EnumLayer.LM_OTHERPLAYER;

        if (m_isMain == false && SceneCamera.m_nModelDetail_Level != 1)
        {
            m_proAvatar.m_nLayer = EnumLayer.LM_DEFAULT;

            m_layer = EnumLayer.LM_DEFAULT;
        }

        PlayerNameUIMgr.getInstance().show(this);
        PlayerNameUIMgr.getInstance().refeshHpColor(this);
        refreshmapCount(mapCount);
        refreshserialCount(serial);
        m_curModel.gameObject.layer = m_layer;
        foreach (Transform tran in m_curGameObj.GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = m_layer;
            if (tran.name == "physics")
            {
                if (m_isMain)
                    tran.gameObject.layer = EnumLayer.LM_BT_SELF;
                else
                    tran.gameObject.layer = EnumLayer.LM_BT_FIGHT;
            } else if (tran.name == "01")
            {
                tran.gameObject.layer = EnumLayer.LM_FX;
            }
        }

        if (m_myPetBird)
        {
            foreach (Transform tran in m_myPetBird.gameObject.GetComponentsInChildren<Transform>())
            {
                tran.gameObject.layer = EnumLayer.LM_MONSTER;
            }
        }
    }

    public override void FrameMove(float delta_time)
    {
        base.FrameMove(delta_time);

        m_proAvatar.FrameMove();

        if (m_bHide_state)
        {
            if (SelfRole.s_bStandaloneScene)
            {
                m_fHideTime -= delta_time;
                if (m_fHideTime <= 0f)
                    LeaveHide();
            }
        }

        if (m_proAvatar.m_Wing_Animstate != null)
        {
            if (m_fWingTime > 0f)
            {
                m_fWingTime -= delta_time;

                if (m_fWingTime <= 0f) m_proAvatar.m_Wing_Animstate.speed = 0.6f;
            }
        }


        //这里可以想象成街机或游戏机的按键
        if (AttackCount > 0f)
        {

            AttackCount -= delta_time;

            this.dianjiTime = NetClient.instance.CurServerTimeStamp;

            if (AttackCount <= 0f)
            {
                //if (m_isMain)
                //{
                //if (!moving)
                //m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 1e-5f, height: 1e-5f);
                //else
                //    m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 0.5f, height: 2f);
                //}
                if (m_curSkillId == 2003)
                    m_moveAgent.speed = 0.125f;

                m_curSkillId = 0;
                m_curAni.SetInteger(EnumAni.ANI_I_SKILL, 0);

                //两个旋转特效bug
                if (this is P2Warrior)
                    m_curModel.gameObject.GetComponent<P2Warrior_Event>().SFX_2003_hide();
                if (this is P5Assassin)
                    m_curModel.gameObject.GetComponent<P5Assassin_Event>().SFX_5003_hide();
            }
        }
        if (isPlayingSkill)
            m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 0.5f, height: 2f);
        else
            m_rshelper.SetNavMeshInfo(avoidancePriority: 20, radius: 1e-5f, height: 1e-5f);


        if (this.dianjiTime != -1 && A3_RideProxy.IsCanChangeRide(this.dianjiTime) && isUp)
        {
            ChangeRideState(true);
        }

        if (m_isMain)
        {
            AutoRide();
        }  

    }


    public bool isPlayingSkill
    {
        get
        {
            if (!can_buff_skill)
                return true;

            if (m_curAni != null)
                return m_curAni.GetInteger(EnumAni.ANI_I_SKILL) != 0 && m_fAttackCount > 0;
            else
                return false;
        }
    }

    public bool isCanPlayingSkill
    {
        get { return !isPlayingSkill && (m_curAni.GetBool(EnumAni.ANI_RUN) || m_curAni); }
    }


    public void PlayHurt()
    {
        if (this.zuoji_Ani == null)
        {
            m_curAni.SetTrigger(EnumAni.ANI_HURT_FRONT);
        }

    }

    public void onDead(bool force = false, BaseRole frm = null)
    {
        if (isDead == false || force)
        {

            isDead = true;

            if (m_curAni)
            {
                m_curAni.enabled = true;
                this.Remove_Ride();
                m_curAni.SetBool(EnumAni.ANI_B_DIE, true);

            }

            if (m_curPhy)
                m_curPhy.gameObject.SetActive(false);

            if (m_moveAgent)
            {

                m_moveAgent.enabled = false;
                m_moveAgent.updateRotation = false;
                //m_moveAgent.updatePosition = false;
            }


            if (m_bUserSelf)
            {
                //根据地图类型判断能否复活
                if (GRMap.curSvrConf.ContainsKey("revive"))
                {
                    bool relive = (int)GRMap.curSvrConf["revive"] > 0 ? true : false;

                    ArrayList list = new ArrayList();
                    list.Add(frm);
                    if (relive) InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_RELIVE, list);
                }
            }
        }
    }

    public void onRelive(int max_hp)
    {
        isDead = false;
        if (m_curAni)
        {
            m_curAni.enabled = true;

            m_curAni.SetBool(EnumAni.ANI_B_DIE, false);

            if (m_isMain && (int)A3_RideModel.getInstance().GetRideInfo().mount == (int)RIDESTATE.UP)
            {
                A3_RideModel.getInstance().GetRideInfo().mount = 0;
                A3_RideProxy.getInstance().SendC2S(4, "mount", 0, true);
            }  ////告诉服务器下坐骑
            else if (m_isMain == false && this.ridestate == RIDESTATE.UP)
            {
                this.ridestate = RIDESTATE.Down;
            }

        }

        if (m_curPhy)
            m_curPhy.gameObject.SetActive(true);

        if (m_moveAgent)
        {
            m_moveAgent.enabled = true;
            m_moveAgent.updateRotation = true;
            m_moveAgent.updatePosition = true;
        }
        maxHp = max_hp;
        curhp = maxHp;
        PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);
        if (m_isMain)
            PlayerModel.getInstance().modHp(curhp);

        //播放特效
        if (m_curModel != null)
        {
            GameObject shield_fx = GAMEAPI.ABFight_LoadPrefab("FX_comFX_fx_player_common_FX_com_fuhuo");
            GameObject fx = GameObject.Instantiate(shield_fx);
            fx.transform.SetParent(m_curModel, false);
            GameObject.Destroy(fx, 4);
        }
    }

    protected int m_testAvatar = 0;
    protected int m_testPro = 1;
    public override void PlaySkill(int id)
    {
        ChangeRideState(false);
        this.dianjiTime = NetClient.instance.CurServerTimeStamp;
    }

    public void PlayJump()
    {
        //if (m_fJumpCount <= 0f) m_curAni.SetBool(EnumAni.ANI_JUMP, true);

        //m_fJumpCount = 0.5f;
    }

    public override void onServerHurt(int damage, int hp, bool dead, BaseRole frm = null, int isCrit = -1, bool miss = false, bool stagger = false)
    {
        if (isDead) return;
        curhp = hp;

        if (stagger)//受伤动作
            PlayHurt();

        if (dead)
        {
            onDead(false, frm);
        }



        if (damage > 0)
        {
            if (!miss)
                PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);
            if (frm != null)
            {
                FightText.CurrentCauseRole = frm;
                if (this == SelfRole._inst && frm != null)
                    FightText.play(FightText.ENEMY_TEXT, getHeadPos(), damage);
                else if (SelfRole._inst.m_unIID == frm.m_unIID)
                {
                    // FightText.play(isCrit ? FightText.CRIT_TEXT : FightText.userText, getHeadPos(), damage);
                    switch (isCrit)
                    {
                        case -1: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                        case 0: break;
                        case 1: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                        case 2: FightText.play(FightText.IMG_TEXT, getHeadPos(), damage, false, isCrit, null, this); break;
                        case 3: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                        case 4: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                        case 5: FightText.play(FightText.userText, getHeadPos(), damage, false, -1, null, this); break;
                        case 6: FightText.play(FightText.IMG_TEXT_2, getHeadPos(), damage, false, isCrit, null, this); break;
                        default: FightText.play(FightText.IMG_TEXT, getHeadPos(), damage, false, isCrit, null, this); break;
                    }
                }
            }
        }


        if (frm != null)
        {
            if (frm is ProfessionRole)
            {
                ProfessionRole pr = frm as ProfessionRole;
                if (pr.invisible)
                    pr.invisible = false;
            }
        }
    }

    public void ShowHurtFX(int id)
    {
        //播放受击特效
        if (id > 0 && id < 10)
        {
            GameObject fx_inst = GameObject.Instantiate(SceneTFX.m_HFX_Prefabs[id], m_curModel.position, m_curModel.rotation) as GameObject;
            fx_inst.transform.SetParent(U3DAPI.FX_POOL_TF, false);
            GameObject.Destroy(fx_inst, 2f);
        }
    }

    public void ShowLvUpFx()
    {
        GameObject fx_inst = GameObject.Instantiate(ROLE_LVUP_FX) as GameObject;
        fx_inst.transform.SetParent(m_curModel, false);
        GameObject.Destroy(fx_inst, 4f);
    }

    public void onHurt(HitData hd)
    {
        if (isDead) return;
        if (hd.m_nDamage == 0)
            return;

        curhp -= hd.m_nDamage;
        if (curhp < 0)
            curhp = 0;
        if (hd.m_nDamage >= 100)
        {
            //if (ConfigUtil.getRandom(1, 10) > 7)
            //    PlayHurt();
        }

        if (curhp == 0)
        {
            //   onDead();
        }

        PlayerNameUIMgr.getInstance().refreshHp(this, curhp, maxHp);

        if (this == SelfRole._inst || SelfRole._inst.m_unIID == hd.m_CastRole.m_unIID)
            FightText.play(FightText.ENEMY_TEXT, lastHeadPos, hd.m_nDamage);
        if (SelfRole.fsm.Autofighting/*在自动战斗*/)
        {
            if (hd.m_CastRole is ProfessionRole/*攻击我的是玩家*/)
            {
                ProfessionRole castRole = (ProfessionRole)hd.m_CastRole;
                switch (PlayerModel.getInstance().pk_state)
                {
                    case PK_TYPE.PK_PKALL:
                        SelfRole.UnderPlayerAttack = true;
                        SelfRole._inst.m_LockRole = hd.m_CastRole;
                        SelfRole.LastAttackPlayer = castRole;
                        break;
                    //case PK_TYPE.PK_HERO:
                    //    if (NameType != REDNAME_TYPE.RNT_NORMAL)
                    //        SelfRole._inst.m_LockRole = hd.m_CastRole;
                    //    else
                    //        SelfRole.UnderPlayerAttack = false;
                    //    break;
                    //case PK_TYPE.PK_LEGION:
                    //    if (castRole.m_unLegionID != SelfRole._inst.m_unLegionID)
                    //        SelfRole._inst.m_LockRole = hd.m_CastRole;
                    //    else
                    //        SelfRole.UnderPlayerAttack = false;
                    //    break;
                    case PK_TYPE.PK_TEAM:
                        if (castRole.m_unTeamID != PlayerModel.getInstance().teamid || !PlayerModel.getInstance().IsInATeam)
                        {
                            SelfRole.UnderPlayerAttack = true;
                            SelfRole._inst.m_LockRole = hd.m_CastRole;
                            SelfRole.LastAttackPlayer = castRole;
                        }
                        else
                        {
                            SelfRole.LastAttackPlayer = null;
                            SelfRole.UnderPlayerAttack = false;
                        }
                        break;
                    case PK_TYPE.PK_PEACE:
                    default:
                        SelfRole.UnderPlayerAttack = false;
                        SelfRole.LastAttackPlayer = null;
                        break;
                }
            }
        }
    }
    public bool can_buff_move = true;
    public bool can_buff_skill = true;
    public bool can_buff_ani = true;
    public bool canMove
    {
        get
        {
            return !MapProxy.getInstance().changingMap && can_buff_move;
        }
    }


    public bool moving;
    public virtual void StartMove(float joy_x, float joy_y)
    {

        if (!canMove)
            return;

        if (isDead) return;

        if (m_fSkillShowTime > 0f) return;

        float next_x = (SceneCamera.m_right.x * joy_x + SceneCamera.m_forward.x * joy_y);
        float next_y = (SceneCamera.m_right.y * joy_x + SceneCamera.m_forward.y * joy_y);

        Vector3 vdir = new Vector3(next_x, 0, next_y);
        m_curModel.forward = vdir;
        moving = true;
        m_curAni.SetBool(EnumAni.ANI_RUN, true);
        if (m_proAvatar != null && m_proAvatar.rideGo != null)
        {
            m_proAvatar.rideGo.GetComponent<Animator>().SetBool(EnumAni.ANI_T_RIDERUN, true);
        }
    }




    public void MoveToTest()
    {
        if (!canMove)
            return;

        float x = 169.6f + UnityEngine.Random.Range(-7f, 7f);
        float z = 98f + UnityEngine.Random.Range(-7f, 7f);

        //float next_x = (SceneCamera.m_right.x * joy_x + SceneCamera.m_forward.x * joy_y);
        //float next_y = (SceneCamera.m_right.y * joy_x + SceneCamera.m_forward.y * joy_y);

        Vector3 vdir = new Vector3(x - m_curModel.position.x, 0, z - m_curModel.position.z);
        m_curModel.forward = vdir;

        m_curAni.SetBool(EnumAni.ANI_RUN, true);
    }

    public void StopMove()
    {
        if (disposed || m_curAni == null)
            return;

        moving = false;
        m_curAni.SetBool(EnumAni.ANI_RUN, false);

        if (m_proAvatar != null && m_proAvatar.rideGo != null)
        {
            m_proAvatar.rideGo.GetComponent<Animator>().SetBool(EnumAni.ANI_T_RIDERUN, false);
        }

    }
    public void refreshtitle(int titile_id)
    {
        PlayerNameUIMgr.getInstance().refreshTitlelv(this, titile_id);
    }

    public void refreshmapCount(int count)
    {
        PlayerNameUIMgr.getInstance().refreshmapCount(this, count, m_isMain);
    }
    public void refreshserialCount(int count)
    {
        PlayerNameUIMgr.getInstance().refreserialCount(this, count);
    }

    public void refreshVipLvl(uint lvl)
    {
        PlayerNameUIMgr.getInstance().refreshVipLv(this, lvl);
    }
    public void refreshnamecolor(int rednm)
    {
        PlayerNameUIMgr.getInstance().refreshNameColor(this, rednm);
    }

    public void refresNameColor_spost(SPOSTNAME_TYPE state)
    {
        PlayerNameUIMgr.getInstance().refresNameColor_spost(this, state);
    }

    public void ChangeRideState(bool boo)
    {
        if (boo)
        {
            if (GRMap.curSvrConf != null && GRMap.curSvrConf.ContainsKey("maptype") && GRMap.curSvrConf["maptype"]._int > 0)
            {
                // 副本中  不上坐骑
            }

            else {

                if (this.invisible == true)  //  || SelfRole.fsm.Autofighting
                {
                    return;  // 状态不上坐骑
                }

                if (m_isMain && A3_RideModel.getInstance().GetRideState() == (int)RIDESTATE.UP && isDead == false)
                {
                    set_Ride(A3_RideModel.getInstance().GetRideId());
                }

                if (m_isMain == false && this.ridestate == RIDESTATE.UP && isDead == false)
                {
                    set_Ride(this.rideId);
                }

            }

            isUp = false;
            this.dianjiTime = -1;
        }
        else {

            if (m_isMain && A3_RideModel.getInstance().GetRideState() == (int)RIDESTATE.UP)
            {
                A3_RideProxy.getInstance().SendC2S(4, "mount", 0, true);

                A3_RideModel.getInstance().GetRideInfo().mount = 0;

            }  // 通知服务器下坐骑

            if (m_isMain == false && this.ridestate == RIDESTATE.UP)
            {
                this.ridestate = RIDESTATE.Down;
            }

            Remove_Ride();

            if (m_isMain && ride_cd.isShow)
            {
                ride_cd.hide();
            }

            isUp = true;

        }//下坐骑

    }

    private int currentStateTime = 0; 

    // 自动寻路状态 自动骑乘
    private void AutoRide() {

        if (FunctionOpenMgr.instance.Check(FunctionOpenMgr.MOUNT) == false )
        {   
            //坐骑功能还未开启 

            return;
        }  

        if (SelfRole.fsm.currentState is StateAutoMoveToPos || SelfRole.fsm.currentState is StateMoveLine)
        {

            if ( currentStateTime == 0 ) currentStateTime = NetClient.instance.CurServerTimeStamp;

            if (A3_RideModel.getInstance().GetRideState() == (int)RIDESTATE.Down)
            {
                int needTime = isUp ? 5 : 2;  // 没有进入战斗状态  时间减少一点

                if (NetClient.instance.CurServerTimeStamp - currentStateTime > needTime)
                {
                    if (isUp)
                    {
                        this.dianjiTime = this.dianjiTime - 5; // 这时候已经算脱离战斗状态了
                    }

                    currentStateTime = NetClient.instance.CurServerTimeStamp;

                    A3_RideProxy.getInstance().SendC2S(4, "mount", 1, false, true);

                }
            }
            else {

                currentStateTime = NetClient.instance.CurServerTimeStamp;

            }

        }
        else {
            currentStateTime =  NetClient.instance.CurServerTimeStamp;
        }

    }
}
